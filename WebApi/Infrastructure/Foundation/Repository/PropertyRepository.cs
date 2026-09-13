using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation.Repository
{
    public class PropertyRepository : EntityRepository<Property>, IPropertyRepository
    {
        public PropertyRepository( BookingDbContext context )
            : base( context )
        {
        }

        public async Task<IReadOnlyList<Property>> GetAllAsync()
        {
            return await Entities
                .AsNoTracking()
                .OrderBy( property => property.Name )
                .ToListAsync();
        }

        public async Task<Property?> GetByIdAsync( Guid id )
        {
            return await Entities.FirstOrDefaultAsync( property => property.Id == id );
        }

        public async Task<IReadOnlyList<Property>> GetByCityAsync( string city )
        {
            string normalizedCity = city.Trim();

            return await Entities
                .AsNoTracking()
                .Where( property => property.City == normalizedCity )
                .OrderBy( property => property.Name )
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync( Guid id )
        {
            return await Entities.AnyAsync( property => property.Id == id );
        }
    }
}