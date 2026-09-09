using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Reservations
{
    public class CancelReservationService : ICancelReservationService
    {
        private readonly IGetReservationService _getReservationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;

        public CancelReservationService(
            IGetReservationService getReservationService,
            IUnitOfWork unitOfWork,
            TimeProvider timeProvider )
        {
            _getReservationService = getReservationService;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }

        public async Task CancelAsync( Guid id )
        {
            Reservation reservation = await _getReservationService.GetByIdAsync( id );

            reservation.Cancel( _timeProvider.GetUtcNow() );

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
