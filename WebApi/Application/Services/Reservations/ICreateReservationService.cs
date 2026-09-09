using Application.Dto;
using Domain.Entities;

namespace Application.Services.Reservations
{
    public interface ICreateReservationService
    {
        Task<Reservation> CreateAsync( NewReservation request );
    }
}
