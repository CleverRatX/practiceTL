using Application.Dto;
using Application.Repositories;
using Application.Rules;
using Application.Services.Availability;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.Search
{
    public class AccommodationSearchService : IAccommodationSearchService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IRoomTypeAvailabilityService _roomTypeAvailabilityService;
        private readonly TimeProvider _timeProvider;

        public AccommodationSearchService(
            IPropertyRepository propertyRepository,
            IRoomTypeRepository roomTypeRepository,
            IRoomTypeAvailabilityService roomTypeAvailabilityService,
            TimeProvider timeProvider )
        {
            _propertyRepository = propertyRepository;
            _roomTypeRepository = roomTypeRepository;
            _roomTypeAvailabilityService = roomTypeAvailabilityService;
            _timeProvider = timeProvider;
        }

        public async Task<IReadOnlyList<SearchResult>> SearchAsync( SearchCriteria criteria )
        {
            Validate( criteria );

            IReadOnlyList<Property> properties = await _propertyRepository.GetByCityAsync( criteria.City );

            if ( properties.Count == 0 )
            {
                return [];
            }

            IReadOnlyList<Guid> propertyIds = properties
                .Select( property => property.Id )
                .ToList();

            IReadOnlyList<RoomType> roomTypes = await _roomTypeRepository.GetByPropertyIdsAsync( propertyIds );

            IReadOnlyList<RoomType> matchingRoomTypes = SelectMatching( roomTypes, criteria );

            IReadOnlyList<RoomType> vacantRoomTypes = await _roomTypeAvailabilityService.SelectVacantAsync(
                matchingRoomTypes,
                criteria.ArrivalDate,
                criteria.DepartureDate );

            Dictionary<Guid, Property> propertiesById = properties.ToDictionary( property => property.Id );
            int nights = BookingRules.CountNights( criteria.ArrivalDate, criteria.DepartureDate );

            return vacantRoomTypes
                .Select( roomType => new SearchResult
                {
                    Property = propertiesById[ roomType.PropertyId ],
                    RoomType = roomType,
                    Nights = nights,
                    Total = BookingRules.CalculateTotal( roomType, nights )
                } )
                .OrderBy( option => option.Total )
                .ToList();
        }

        private static IReadOnlyList<RoomType> SelectMatching( IReadOnlyList<RoomType> roomTypes, SearchCriteria criteria )
        {
            IEnumerable<RoomType> matching = roomTypes.Where( roomType => BookingRules.FitsGuests( roomType, criteria.GuestCount ) );

            if ( criteria.MaxDailyPrice.HasValue )
            {
                decimal maxDailyPrice = criteria.MaxDailyPrice.Value;
                matching = matching.Where( roomType => roomType.DailyPrice <= maxDailyPrice );
            }

            return matching.ToList();
        }

        private void Validate( SearchCriteria criteria )
        {
            if ( string.IsNullOrWhiteSpace( criteria.City ) )
            {
                throw new DomainValidationException( "Город обязателен для поиска." );
            }

            if ( criteria.DepartureDate <= criteria.ArrivalDate )
            {
                throw new DomainValidationException( "Дата выезда должна быть позже даты заезда." );
            }

            DateOnly today = DateOnly.FromDateTime( _timeProvider.GetUtcNow().UtcDateTime );

            if ( criteria.ArrivalDate < today )
            {
                throw new DomainValidationException( "Дата заезда не может быть в прошлом." );
            }

            if ( criteria.GuestCount <= 0 )
            {
                throw new DomainValidationException( "Количество гостей должно быть больше нуля." );
            }

            if ( criteria.MaxDailyPrice.HasValue && criteria.MaxDailyPrice.Value <= 0 )
            {
                throw new DomainValidationException( "Максимальная цена должна быть больше нуля." );
            }
        }
    }
}