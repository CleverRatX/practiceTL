using Domain.Entities;
using Domain.Models;

namespace Domain.Services
{
    public interface IRoomTypeService
    {
        Task<IReadOnlyList<RoomType>> GetByPropertyAsync( Guid propertyId, CancellationToken cancellationToken );

        Task<RoomType> GetByIdAsync( Guid id, CancellationToken cancellationToken );

        Task<RoomType> CreateAsync( Guid propertyId, RoomTypeData data, CancellationToken cancellationToken );

        Task<RoomType> UpdateAsync( Guid id, RoomTypeData data, CancellationToken cancellationToken );

        Task DeleteAsync( Guid id, CancellationToken cancellationToken );
    }
}
