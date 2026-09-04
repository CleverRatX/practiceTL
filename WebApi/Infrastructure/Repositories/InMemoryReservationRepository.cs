using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Storage;

namespace Infrastructure.Repositories
{
    public class InMemoryReservationRepository : IReservationRepository
    {
        private readonly InMemoryStorage _storage;

        public InMemoryReservationRepository( InMemoryStorage storage )
        {
            _storage = storage;
        }

        public Task<Reservation?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.Reservations.TryGetValue( id, out Reservation? reservation );

            return Task.FromResult( reservation );
        }

        public Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<Reservation> reservations = _storage.Reservations.Values;

            if ( !filter.IncludeCancelled )
            {
                reservations = reservations.Where( reservation => reservation.IsActive );
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
                reservations = reservations.Where(
                    reservation => reservation.GuestName.Contains( guestName, StringComparison.OrdinalIgnoreCase ) );
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

            IReadOnlyList<Reservation> result = reservations
                .OrderBy( reservation => reservation.ArrivalDate )
                .ThenBy( reservation => reservation.CreatedAt )
                .ToList();

            return Task.FromResult( result );
        }

        public Task<IReadOnlyList<Reservation>> GetOverlappingAsync(
            Guid roomTypeId,
            DateOnly arrivalDate,
            DateOnly departureDate,
            CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<Reservation> reservations = _storage.Reservations.Values
                .Where( reservation => reservation.IsActive
                    && reservation.RoomTypeId == roomTypeId
                    && reservation.ArrivalDate < departureDate
                    && reservation.DepartureDate > arrivalDate )
                .ToList();

            return Task.FromResult( reservations );
        }

        public Task<bool> HasActiveByPropertyAsync( Guid propertyId, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool hasReservations = _storage.Reservations.Values
                .Any( reservation => reservation.IsActive && reservation.PropertyId == propertyId );

            return Task.FromResult( hasReservations );
        }

        public Task<bool> HasActiveByRoomTypeAsync( Guid roomTypeId, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool hasReservations = _storage.Reservations.Values
                .Any( reservation => reservation.IsActive && reservation.RoomTypeId == roomTypeId );

            return Task.FromResult( hasReservations );
        }

        public Task AddAsync( Reservation reservation, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.Reservations[ reservation.Id ] = reservation;

            return Task.CompletedTask;
        }

        public Task UpdateAsync( Reservation reservation, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.Reservations[ reservation.Id ] = reservation;

            return Task.CompletedTask;
        }
    }
}
