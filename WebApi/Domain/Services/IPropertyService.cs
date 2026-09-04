using Domain.Entities;
using Domain.Models;

namespace Domain.Services
{
    public interface IPropertyService
    {
        Task<IReadOnlyList<Property>> GetAllAsync( CancellationToken cancellationToken );

        Task<Property> GetByIdAsync( Guid id, CancellationToken cancellationToken );

        Task<Property> CreateAsync( PropertyData data, CancellationToken cancellationToken );

        Task<Property> UpdateAsync( Guid id, PropertyData data, CancellationToken cancellationToken );

        Task DeleteAsync( Guid id, CancellationToken cancellationToken );
    }
}
