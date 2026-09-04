using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPropertyRepository
    {
        Task<IReadOnlyList<Property>> GetAllAsync( CancellationToken cancellationToken );

        Task<Property?> GetByIdAsync( Guid id, CancellationToken cancellationToken );

        Task<IReadOnlyList<Property>> GetByCityAsync( string city, CancellationToken cancellationToken );

        Task AddAsync( Property property, CancellationToken cancellationToken );

        Task UpdateAsync( Property property, CancellationToken cancellationToken );

        Task DeleteAsync( Guid id, CancellationToken cancellationToken );
    }
}
