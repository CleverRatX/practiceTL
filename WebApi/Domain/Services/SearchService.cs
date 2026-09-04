using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;

namespace Domain.Services
{
    public class SearchService : ISearchService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IAvailabilityService _availabilityService;
        private readonly TimeProvider _timeProvider;

        public SearchService(
            IPropertyRepository propertyRepository,
            IRoomTypeRepository roomTypeRepository,
            IAvailabilityService availabilityService,
            TimeProvider timeProvider )
        {
            _propertyRepository = propertyRepository;
            _roomTypeRepository = roomTypeRepository;
            _availabilityService = availabilityService;
            _timeProvider = timeProvider;
        }

        public async Task<IReadOnlyList<SearchResult>> SearchAsync( SearchCriteria criteria, CancellationToken cancellationToken )
        {
            Validate( criteria );

            int nights = criteria.DepartureDate.DayNumber - criteria.ArrivalDate.DayNumber;

            IReadOnlyList<Property> properties = await _propertyRepository.GetByCityAsync( criteria.City, cancellationToken );

            List<SearchResult> options = new();

            foreach ( Property property in properties )
            {
                IReadOnlyList<RoomType> roomTypes = await _roomTypeRepository.GetByPropertyIdAsync( property.Id, cancellationToken );

                foreach ( RoomType roomType in SelectSuitable( roomTypes, criteria ) )
                {
                    bool isAvailable = await _availabilityService.IsAvailableAsync(
                        roomType,
                        criteria.ArrivalDate,
                        criteria.DepartureDate,
                        cancellationToken );

                    if ( !isAvailable )
                    {
                        continue;
                    }

                    options.Add( new SearchResult( property, roomType, nights, roomType.CalculateTotal( nights ) ) );
                }
            }

            return options
                .OrderBy( option => option.Total )
                .ToList();
        }

        private static IEnumerable<RoomType> SelectSuitable( IReadOnlyList<RoomType> roomTypes, SearchCriteria criteria )
        {
            IEnumerable<RoomType> suitable = roomTypes.Where( roomType => roomType.FitsGuests( criteria.GuestCount ) );

            if ( criteria.MaxDailyPrice.HasValue )
            {
                decimal maxDailyPrice = criteria.MaxDailyPrice.Value;
                suitable = suitable.Where( roomType => roomType.DailyPrice <= maxDailyPrice );
            }

            return suitable;
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
