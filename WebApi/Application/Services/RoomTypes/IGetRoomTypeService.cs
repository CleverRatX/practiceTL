using Domain.Entities;

namespace Application.Services.RoomTypes
{
    public interface IGetRoomTypeService
    {
        Task<RoomType> GetByIdAsync( Guid id );
    }
}
