using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation.Repository
{
    public abstract class EntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected BookingDbContext Context { get; }

        protected DbSet<TEntity> Entities => Context.Set<TEntity>();

        protected EntityRepository( BookingDbContext context )
        {
            Context = context;
        }

        public void Add( TEntity entity )
        {
            Entities.Add( entity );
        }

        public void Add( IEnumerable<TEntity> entities )
        {
            Entities.AddRange( entities );
        }

        public void Remove( TEntity entity )
        {
            Entities.Remove( entity );
        }

        public void Remove( IEnumerable<TEntity> entities )
        {
            Entities.RemoveRange( entities );
        }
    }
}
