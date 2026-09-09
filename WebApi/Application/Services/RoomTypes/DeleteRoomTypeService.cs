using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services.RoomTypes
{
    public class DeleteRoomTypeService : IDeleteRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomTypeService(
            IRoomTypeRepository roomTypeRepository,
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork )
        {
            _roomTypeRepository = roomTypeRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteAsync( Guid id )
        {
            RoomType? roomType = await _roomTypeRepository.GetByIdAsync( id );

            if ( roomType is null )
            {
                return;
            }

            bool hasActiveReservations = await _reservationRepository.HasActiveByRoomTypeIdAsync( id );

            if ( hasActiveReservations )
            {
                throw new ConflictException(
                    "Нельзя удалить категорию номера, по которой есть действующие бронирования." );
            }

            _roomTypeRepository.Remove( roomType );
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
