using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Storage;

namespace Infrastructure.Repositories
{
    public class InMemoryRoomTypeRepository : IRoomTypeRepository
    {
        private readonly InMemoryStorage _storage;

        public InMemoryRoomTypeRepository( InMemoryStorage storage )
        {
            _storage = storage;
        }

        public Task<RoomType?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.RoomTypes.TryGetValue( id, out RoomType? roomType );

            return Task.FromResult( roomType );
        }

        public Task<IReadOnlyList<RoomType>> GetByPropertyIdAsync( Guid propertyId, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<RoomType> roomTypes = _storage.RoomTypes.Values
                .Where( roomType => roomType.PropertyId == propertyId )
                .OrderBy( roomType => roomType.DailyPrice )
                .ToList();

            return Task.FromResult( roomTypes );
        }

        public Task AddAsync( RoomType roomType, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.RoomTypes[ roomType.Id ] = roomType;

            return Task.CompletedTask;
        }

        public Task UpdateAsync( RoomType roomType, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.RoomTypes[ roomType.Id ] = roomType;

            return Task.CompletedTask;
        }

        public Task DeleteAsync( Guid id, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            _storage.RoomTypes.TryRemove( id, out _ );

            return Task.CompletedTask;
        }

        public Task DeleteByPropertyIdAsync( Guid propertyId, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<Guid> ids = _storage.RoomTypes.Values
                .Where( roomType => roomType.PropertyId == propertyId )
                .Select( roomType => roomType.Id )
                .ToList();

            foreach ( Guid id in ids )
            {
                _storage.RoomTypes.TryRemove( id, out _ );
            }

            return Task.CompletedTask;
        }
    }
}
