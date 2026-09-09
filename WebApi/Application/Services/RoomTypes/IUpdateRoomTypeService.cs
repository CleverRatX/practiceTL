using Application.Dto;
using Domain.Entities;

namespace Application.Services.RoomTypes
{
    public interface IUpdateRoomTypeService
    {
        Task<RoomType> UpdateAsync( Guid id, RoomTypeData data );
    }
}
