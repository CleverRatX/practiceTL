using Application.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.Properties
{
    public class DeletePropertyService : IDeletePropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyService(
            IPropertyRepository propertyRepository,
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork )
        {
            _propertyRepository = propertyRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteAsync( Guid id )
        {
            Property? property = await _propertyRepository.GetByIdAsync( id );

            if ( property is null )
            {
                return;
            }

            bool hasActiveReservations = await _reservationRepository.HasActiveByPropertyIdAsync( id );

            if ( hasActiveReservations )
            {
                throw new ConflictException(
                    "Нельзя удалить средство размещения, по которому есть действующие бронирования." );
            }

            _propertyRepository.Remove( property );
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
