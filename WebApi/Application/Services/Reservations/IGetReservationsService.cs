using Domain.Entities;
using Domain.Queries;

namespace Application.Services.Reservations
{
    public interface IGetReservationsService
    {
        Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter );
    }
}
