using Domain.Entities;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation.Repository
{
    public class ReservationRepository : EntityRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository( BookingDbContext context )
            : base( context )
        {
        }

        public async Task<Reservation?> GetByIdAsync( Guid id )
        {
            return await Entities.FirstOrDefaultAsync( reservation => reservation.Id == id );
        }

        public async Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter )
        {
            IQueryable<Reservation> reservations = Entities.AsNoTracking();

            if ( !filter.IncludeCancelled )
            {
                reservations = reservations.Where( reservation => reservation.Status == ReservationStatus.Active );
            }

            if ( filter.PropertyId.HasValue )
            {
                Guid propertyId = filter.PropertyId.Value;
                reservations = reservations.Where( reservation => reservation.PropertyId == propertyId );
            }

            if ( filter.RoomTypeId.HasValue )
            {
                Guid roomTypeId = filter.RoomTypeId.Value;
                reservations = reservations.Where( reservation => reservation.RoomTypeId == roomTypeId );
            }

            if ( !string.IsNullOrWhiteSpace( filter.GuestName ) )
            {
                string guestName = filter.GuestName.Trim();
                reservations = reservations.Where( reservation => reservation.GuestName.Contains( guestName ) );
            }

            if ( filter.ArrivalDateFrom.HasValue )
            {
                DateOnly arrivalDateFrom = filter.ArrivalDateFrom.Value;
                reservations = reservations.Where( reservation => reservation.ArrivalDate >= arrivalDateFrom );
            }

            if ( filter.DepartureDateTo.HasValue )
            {
                DateOnly departureDateTo = filter.DepartureDateTo.Value;
                reservations = reservations.Where( reservation => reservation.DepartureDate <= departureDateTo );
            }

            return await reservations
                .OrderBy( reservation => reservation.ArrivalDate )
                .ThenBy( reservation => reservation.CreatedAt )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Reservation>> GetOverlappingAsync(
            Guid roomTypeId,
            DateOnly arrivalDate,
            DateOnly departureDate )
        {
            return await Entities
                .AsNoTracking()
                .Where( reservation => reservation.Status == ReservationStatus.Active
                    && reservation.RoomTypeId == roomTypeId
                    && reservation.ArrivalDate < departureDate
                    && reservation.DepartureDate > arrivalDate )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Reservation>> GetOverlappingAsync(
            IReadOnlyList<Guid> roomTypeIds,
            DateOnly arrivalDate,
            DateOnly departureDate )
        {
            return await Entities
                .AsNoTracking()
                .Where( reservation => reservation.Status == ReservationStatus.Active
                    && roomTypeIds.Contains( reservation.RoomTypeId )
                    && reservation.ArrivalDate < departureDate
                    && reservation.DepartureDate > arrivalDate )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Reservation>> GetActiveByRoomTypeIdAsync( Guid roomTypeId, DateOnly fromDate )
        {
            return await Entities
                .AsNoTracking()
                .Where( reservation => reservation.Status == ReservationStatus.Active
                    && reservation.RoomTypeId == roomTypeId
                    && reservation.DepartureDate > fromDate )
                .ToListAsync();
        }

        public async Task<bool> HasActiveByPropertyIdAsync( Guid propertyId )
        {
            return await Entities.AnyAsync( reservation => reservation.Status == ReservationStatus.Active
                && reservation.PropertyId == propertyId );
        }

        public async Task<bool> HasActiveByRoomTypeIdAsync( Guid roomTypeId )
        {
            return await Entities.AnyAsync( reservation => reservation.Status == ReservationStatus.Active
                && reservation.RoomTypeId == roomTypeId );
        }
    }
}
