using Domain.Repositories;
using Domain.Entities;
using Domain.Queries;

namespace Application.Services.Reservations
{
    public class GetReservationsService : IGetReservationsService
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationsService( IReservationRepository reservationRepository )
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter )
        {
            return await _reservationRepository.GetAsync( filter );
        }
    }
}
