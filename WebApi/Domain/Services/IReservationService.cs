using Domain.Entities;
using Domain.Models;

namespace Domain.Services
{
    public interface IReservationService
    {
        Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter, CancellationToken cancellationToken );

        Task<Reservation> GetByIdAsync( Guid id, CancellationToken cancellationToken );

        Task<Reservation> CreateAsync( NewReservation request, CancellationToken cancellationToken );

        Task CancelAsync( Guid id, CancellationToken cancellationToken );
    }
}
