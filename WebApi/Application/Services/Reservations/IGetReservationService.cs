using Domain.Entities;

namespace Application.Services.Reservations
{
    public interface IGetReservationService
    {
        Task<Reservation> GetByIdAsync( Guid id );
    }
}
