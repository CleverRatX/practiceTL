using Domain.Entities;

namespace Application.Services.RoomTypes
{
    public interface IGetPropertyRoomTypesService
    {
        Task<IReadOnlyList<RoomType>> GetByPropertyIdAsync( Guid propertyId );
    }
}
