using Domain.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.Reservations
{
    public class GetReservationService : IGetReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationService( IReservationRepository reservationRepository )
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<Reservation> GetByIdAsync( Guid id )
        {
            Reservation? reservation = await _reservationRepository.GetByIdAsync( id );

            if ( reservation is null )
            {
                throw new EntityNotFoundException( $"Бронирование с id: {id} не найдено." );
            }

            return reservation;
        }
    }
}
