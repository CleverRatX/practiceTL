using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Reservations
{
    public interface IGetReservationsService
    {
        Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter );
    }
}
