using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;

namespace Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IAvailabilityService _availabilityService;
        private readonly TimeProvider _timeProvider;

        public ReservationService(
            IReservationRepository reservationRepository,
            IPropertyRepository propertyRepository,
            IRoomTypeRepository roomTypeRepository,
            IAvailabilityService availabilityService,
            TimeProvider timeProvider )
        {
            _reservationRepository = reservationRepository;
            _propertyRepository = propertyRepository;
            _roomTypeRepository = roomTypeRepository;
            _availabilityService = availabilityService;
            _timeProvider = timeProvider;
        }

        public Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter, CancellationToken cancellationToken )
        {
            return _reservationRepository.GetAsync( filter, cancellationToken );
        }

        public async Task<Reservation> GetByIdAsync( Guid id, CancellationToken cancellationToken )
        {
            Reservation? reservation = await _reservationRepository.GetByIdAsync( id, cancellationToken );

            if ( reservation is null )
            {
                throw new EntityNotFoundException( $"Бронирование {id} не найдено." );
            }

            return reservation;
        }

        public async Task<Reservation> CreateAsync( NewReservation request, CancellationToken cancellationToken )
        {
            Property? property = await _propertyRepository.GetByIdAsync( request.PropertyId, cancellationToken );

            if ( property is null )
            {
                throw new EntityNotFoundException( $"Средство размещения {request.PropertyId} не найдено." );
            }

            RoomType? roomType = await _roomTypeRepository.GetByIdAsync( request.RoomTypeId, cancellationToken );

            if ( roomType is null )
            {
                throw new EntityNotFoundException( $"Категория номера {request.RoomTypeId} не найдена." );
            }

            if ( roomType.PropertyId != property.Id )
            {
                throw new DomainValidationException( "Выбранная категория номера принадлежит другому средству размещения." );
            }

            ValidateArrivalIsNotInPast( request );

            if ( !roomType.FitsGuests( request.GuestCount ) )
            {
                throw new DomainValidationException(
                    $"Категория номера рассчитана на {roomType.MinPersonCount}-{roomType.MaxPersonCount} гостей." );
            }

            bool isAvailable = await _availabilityService.IsAvailableAsync(
                roomType,
                request.ArrivalDate,
                request.DepartureDate,
                cancellationToken );

            if ( !isAvailable )
            {
                throw new ConflictException( "На выбранные даты свободных номеров этой категории нет." );
            }

            int nights = request.DepartureDate.DayNumber - request.ArrivalDate.DayNumber;
            decimal total = roomType.CalculateTotal( nights );

            Reservation reservation = new( request, total, roomType.Currency, _timeProvider.GetUtcNow() );

            await _reservationRepository.AddAsync( reservation, cancellationToken );

            return reservation;
        }

        public async Task CancelAsync( Guid id, CancellationToken cancellationToken )
        {
            Reservation reservation = await GetByIdAsync( id, cancellationToken );

            reservation.Cancel( _timeProvider.GetUtcNow() );

            await _reservationRepository.UpdateAsync( reservation, cancellationToken );
        }

        private void ValidateArrivalIsNotInPast( NewReservation request )
        {
            DateOnly today = DateOnly.FromDateTime( _timeProvider.GetUtcNow().UtcDateTime );

            if ( request.ArrivalDate < today )
            {
                throw new DomainValidationException( "Дата заезда не может быть в прошлом." );
            }
        }
    }
}
