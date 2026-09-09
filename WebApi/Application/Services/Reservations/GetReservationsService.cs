using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Reservations
{
    public class GetReservationsService : IGetReservationsService
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationsService( IReservationRepository reservationRepository )
        {
            _reservationRepository = reservationRepository;
        }

        public Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter )
        {
            return _reservationRepository.GetAsync( filter );
        }
    }
}
