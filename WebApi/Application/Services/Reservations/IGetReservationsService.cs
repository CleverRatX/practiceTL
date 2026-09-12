using Application.Dto;
using Application.Repositories;
using Domain.Entities;

namespace Application.Services.Reservations
{
    public interface IGetReservationsService
    {
        Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter );
    }
}
