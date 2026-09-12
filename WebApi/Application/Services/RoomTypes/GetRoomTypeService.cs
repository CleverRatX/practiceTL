using Application.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.RoomTypes
{
    public class GetRoomTypeService : IGetRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public GetRoomTypeService( IRoomTypeRepository roomTypeRepository )
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<RoomType> GetByIdAsync( Guid id )
        {
            RoomType? roomType = await _roomTypeRepository.GetByIdAsync( id );

            if ( roomType is null )
            {
                throw new EntityNotFoundException( $"Категория номера с id: {id} не найдена." );
            }

            return roomType;
        }
    }
}
