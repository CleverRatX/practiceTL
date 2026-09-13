using System.Data;
using Application.Dto;
using Domain.Repositories;
using Application.Rules;
using Application.Persistence;
using Application.Services.Availability;
using Application.Services.RoomTypes;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.Reservations
{
    public class CreateReservationService : ICreateReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IGetRoomTypeService _getRoomTypeService;
        private readonly IRoomTypeAvailabilityService _roomTypeAvailabilityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;

        public CreateReservationService(
            IReservationRepository reservationRepository,
            IGetRoomTypeService getRoomTypeService,
            IRoomTypeAvailabilityService roomTypeAvailabilityService,
            IUnitOfWork unitOfWork,
            TimeProvider timeProvider )
        {
            _reservationRepository = reservationRepository;
            _getRoomTypeService = getRoomTypeService;
            _roomTypeAvailabilityService = roomTypeAvailabilityService;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }

        public async Task<Reservation> CreateAsync( NewReservation request )
        {
            ValidateDates( request );

            await using ITransaction transaction = await _unitOfWork.BeginTransactionAsync( IsolationLevel.Serializable );

            RoomType roomType = await _getRoomTypeService.GetByIdAsync( request.RoomTypeId );

            if ( roomType.PropertyId != request.PropertyId )
            {
                throw new DomainValidationException(
                    "Выбранная категория номера принадлежит другому средству размещения." );
            }

            if ( !BookingRules.FitsGuests( roomType, request.GuestCount ) )
            {
                throw new DomainValidationException(
                    $"Категория номера рассчитана на {roomType.MinPersonCount}-{roomType.MaxPersonCount} гостей." );
            }

            bool hasVacantRooms = await _roomTypeAvailabilityService.HasVacantRoomsAsync(
                roomType,
                request.ArrivalDate,
                request.DepartureDate );

            if ( !hasVacantRooms )
            {
                throw new ConflictException( "На выбранные даты свободных номеров этой категории нет." );
            }

            int nights = BookingRules.CountNights( request.ArrivalDate, request.DepartureDate );

            Reservation reservation = new(
                roomType.PropertyId,
                roomType.Id,
                request.ArrivalDate,
                request.DepartureDate,
                BookingRules.ArrivalTime,
                BookingRules.DepartureTime,
                request.GuestName,
                request.GuestPhoneNumber,
                request.GuestCount,
                BookingRules.CalculateTotal( roomType, nights ),
                roomType.Currency,
                _timeProvider.GetUtcNow() );

            _reservationRepository.Add( reservation );

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return reservation;
        }

        private void ValidateDates( NewReservation request )
        {
            if ( request.DepartureDate <= request.ArrivalDate )
            {
                throw new DomainValidationException( "Дата выезда должна быть позже даты заезда." );
            }

            DateOnly today = DateOnly.FromDateTime( _timeProvider.GetUtcNow().UtcDateTime );

            if ( request.ArrivalDate < today )
            {
                throw new DomainValidationException( "Дата заезда не может быть в прошлом." );
            }
        }
    }
}