using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IReservationRepository _reservationRepository;

        public AvailabilityService( IReservationRepository reservationRepository )
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<bool> IsAvailableAsync(
            RoomType roomType,
            DateOnly arrivalDate,
            DateOnly departureDate,
            CancellationToken cancellationToken )
        {
            IReadOnlyList<Reservation> reservations = await _reservationRepository.GetOverlappingAsync(
                roomType.Id,
                arrivalDate,
                departureDate,
                cancellationToken );

            if ( reservations.Count == 0 )
            {
                return true;
            }

            for ( DateOnly night = arrivalDate; night < departureDate; night = night.AddDays( 1 ) )
            {
                int occupiedRooms = reservations.Count( reservation => reservation.CoversNight( night ) );

                if ( occupiedRooms >= roomType.RoomCount )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
