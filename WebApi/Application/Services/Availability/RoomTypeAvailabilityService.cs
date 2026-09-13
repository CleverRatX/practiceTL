using Application.Rules;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Availability
{
    public class RoomTypeAvailabilityService : IRoomTypeAvailabilityService
    {
        private readonly IReservationRepository _reservationRepository;

        public RoomTypeAvailabilityService( IReservationRepository reservationRepository )
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<bool> HasVacantRoomsAsync( RoomType roomType, DateOnly arrivalDate, DateOnly departureDate )
        {
            IReadOnlyList<Reservation> reservations = await _reservationRepository.GetOverlappingAsync(
                roomType.Id,
                arrivalDate,
                departureDate );

            return HasVacantRooms( roomType, reservations, arrivalDate, departureDate );
        }

        public async Task<IReadOnlyList<RoomType>> SelectVacantAsync(
            IReadOnlyList<RoomType> roomTypes,
            DateOnly arrivalDate,
            DateOnly departureDate )
        {
            if ( roomTypes.Count == 0 )
            {
                return [];
            }

            IReadOnlyList<Guid> roomTypeIds = roomTypes
                .Select( roomType => roomType.Id )
                .ToList();

            IReadOnlyList<Reservation> reservations = await _reservationRepository.GetOverlappingAsync(
                roomTypeIds,
                arrivalDate,
                departureDate );

            ILookup<Guid, Reservation> reservationsByRoomType = reservations.ToLookup( reservation => reservation.RoomTypeId );

            List<RoomType> vacantRoomTypes = new();

            foreach ( RoomType roomType in roomTypes )
            {
                IReadOnlyList<Reservation> roomTypeReservations = reservationsByRoomType[ roomType.Id ].ToList();

                if ( HasVacantRooms( roomType, roomTypeReservations, arrivalDate, departureDate ) )
                {
                    vacantRoomTypes.Add( roomType );
                }
            }

            return vacantRoomTypes;
        }

        private static bool HasVacantRooms(
            RoomType roomType,
            IReadOnlyList<Reservation> reservations,
            DateOnly arrivalDate,
            DateOnly departureDate )
        {
            if ( reservations.Count == 0 )
            {
                return true;
            }

            return BookingRules.CountPeakOccupancy( reservations, arrivalDate, departureDate ) < roomType.RoomCount;
        }
    }
}