using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation.Repository
{
    public class RoomTypeRepository : EntityRepository<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository( BookingDbContext context )
            : base( context )
        {
        }

        public Task<RoomType?> GetByIdAsync( Guid id )
        {
            return Entities.FirstOrDefaultAsync( roomType => roomType.Id == id );
        }

        public Task<RoomType?> GetByIdForUpdateAsync( Guid id )
        {
            return Entities
                .FromSql( $"SELECT * FROM [RoomTypes] WITH (UPDLOCK, ROWLOCK) WHERE [Id] = {id}" )
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<RoomType>> GetByPropertyIdAsync( Guid propertyId )
        {
            return await Entities
                .AsNoTracking()
                .Where( roomType => roomType.PropertyId == propertyId )
                .OrderBy( roomType => roomType.DailyPrice )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<RoomType>> GetByPropertyIdsAsync( IReadOnlyList<Guid> propertyIds )
        {
            return await Entities
                .AsNoTracking()
                .Where( roomType => propertyIds.Contains( roomType.PropertyId ) )
                .OrderBy( roomType => roomType.DailyPrice )
                .ToListAsync();
        }
    }
}