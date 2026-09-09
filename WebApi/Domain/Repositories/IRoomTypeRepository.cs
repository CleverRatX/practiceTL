using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        Task<RoomType?> GetByIdAsync( Guid id );

        Task<RoomType?> GetByIdForUpdateAsync( Guid id );

        Task<IReadOnlyList<RoomType>> GetByPropertyIdAsync( Guid propertyId );

        Task<IReadOnlyList<RoomType>> GetByPropertyIdsAsync( IReadOnlyList<Guid> propertyIds );
    }
}