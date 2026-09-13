using Application.Dto;
using Domain.Entities;

namespace Application.Services.RoomTypes
{
    public interface ICreateRoomTypeService
    {
        Task<RoomType> CreateAsync( Guid propertyId, RoomTypeData data );
    }
}
