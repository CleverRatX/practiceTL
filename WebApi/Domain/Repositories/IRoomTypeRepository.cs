using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRoomTypeRepository
    {
        Task<RoomType?> GetByIdAsync( Guid id, CancellationToken cancellationToken );

        Task<IReadOnlyList<RoomType>> GetByPropertyIdAsync( Guid propertyId, CancellationToken cancellationToken );

        Task AddAsync( RoomType roomType, CancellationToken cancellationToken );

        Task UpdateAsync( RoomType roomType, CancellationToken cancellationToken );

        Task DeleteAsync( Guid id, CancellationToken cancellationToken );

        Task DeleteByPropertyIdAsync( Guid propertyId, CancellationToken cancellationToken );
    }
}
