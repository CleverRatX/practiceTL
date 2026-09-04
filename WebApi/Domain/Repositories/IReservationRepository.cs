using Domain.Entities;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync( Guid id, CancellationToken cancellationToken );

        Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter, CancellationToken cancellationToken );

        Task<IReadOnlyList<Reservation>> GetOverlappingAsync(
            Guid roomTypeId,
            DateOnly arrivalDate,
            DateOnly departureDate,
            CancellationToken cancellationToken );

        Task<bool> HasActiveByPropertyAsync( Guid propertyId, CancellationToken cancellationToken );

        Task<bool> HasActiveByRoomTypeAsync( Guid roomTypeId, CancellationToken cancellationToken );

        Task AddAsync( Reservation reservation, CancellationToken cancellationToken );

        Task UpdateAsync( Reservation reservation, CancellationToken cancellationToken );
    }
}
