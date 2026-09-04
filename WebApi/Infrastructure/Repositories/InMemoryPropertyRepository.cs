using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Storage;

namespace Infrastructure.Repositories
{
    public class InMemoryPropertyRepository : IPropertyRepository
    {
        private readonly InMemoryStorage _storage;

        public InMemoryPropertyRepository( InMemoryStorage storage )
        {
            _storage = storage;
        }

        public Task<IReadOnlyList<Property>> GetAllAsync( CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<Property> properties = _storage.Properties.Values
                .OrderBy( property => property.Name )
                .ToList();

            return Task.FromResult( properties );
        }

        public Task<Property?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.Properties.TryGetValue( id, out Property? property );

            return Task.FromResult( property );
        }

        public Task<IReadOnlyList<Property>> GetByCityAsync( string city, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<Property> properties = _storage.Properties.Values
                .Where( property => string.Equals( property.City, city.Trim(), StringComparison.OrdinalIgnoreCase ) )
                .OrderBy( property => property.Name )
                .ToList();

            return Task.FromResult( properties );
        }

        public Task AddAsync( Property property, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.Properties[ property.Id ] = property;

            return Task.CompletedTask;
        }

        public Task UpdateAsync( Property property, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.Properties[ property.Id ] = property;

            return Task.CompletedTask;
        }

        public Task DeleteAsync( Guid id, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.Properties.TryRemove( id, out _ );

            return Task.CompletedTask;
        }
    }
}
