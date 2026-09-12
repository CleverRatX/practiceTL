using Application.Dto;
using Application.Repositories;
using Application.Rules;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.RoomTypes
{
    public class UpdateRoomTypeService : IUpdateRoomTypeService
    {
        private readonly IGetRoomTypeService _getRoomTypeService;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;

        public UpdateRoomTypeService(
            IGetRoomTypeService getRoomTypeService,
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork,
            TimeProvider timeProvider )
        {
            _getRoomTypeService = getRoomTypeService;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }

        public async Task<RoomType> UpdateAsync( Guid id, RoomTypeData data )
        {
            RoomType roomType = await _getRoomTypeService.GetByIdAsync( id );

            DateOnly today = DateOnly.FromDateTime( _timeProvider.GetUtcNow().UtcDateTime );

            IReadOnlyList<Reservation> reservations = await _reservationRepository.GetActiveByRoomTypeIdAsync( id, today );

            ValidateAgainstReservations( reservations, data, today );

            roomType.Update(
                data.Name,
                data.DailyPrice,
                data.Currency,
                data.MinPersonCount,
                data.MaxPersonCount,
                data.RoomCount,
                data.Services,
                data.Amenities );

            await _unitOfWork.SaveChangesAsync();

            return roomType;
        }

        private static void ValidateAgainstReservations(
            IReadOnlyList<Reservation> reservations,
            RoomTypeData data,
            DateOnly today )
        {
            if ( reservations.Count == 0 )
            {
                return;
            }

            DateOnly lastDepartureDate = reservations.Max( reservation => reservation.DepartureDate );
            int peakOccupancy = BookingRules.CountPeakOccupancy( reservations, today, lastDepartureDate );

            if ( data.RoomCount < peakOccupancy )
            {
                throw new ConflictException(
                    $"Нельзя оставить {data.RoomCount} номеров: на действующие брони нужно минимум {peakOccupancy}." );
            }

            int minGuestCount = reservations.Min( reservation => reservation.GuestCount );
            int maxGuestCount = reservations.Max( reservation => reservation.GuestCount );

            if ( data.MinPersonCount > minGuestCount || data.MaxPersonCount < maxGuestCount )
            {
                throw new ConflictException(
                    $"Есть действующие брони на {minGuestCount}-{maxGuestCount} гостей, "
                    + $"новые границы вместимости {data.MinPersonCount}-{data.MaxPersonCount} их не покрывают." );
            }
        }
    }
}