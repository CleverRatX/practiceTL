using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPropertyRepository : IRepository<Property>
    {
        Task<IReadOnlyList<Property>> GetAllAsync();

        Task<Property?> GetByIdAsync( Guid id );

        Task<IReadOnlyList<Property>> GetByCityAsync( string city );

        Task<bool> ExistsAsync( Guid id );
    }
}
