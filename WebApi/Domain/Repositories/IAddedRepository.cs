namespace Domain.Repositories
{
    public interface IAddedRepository<TEntity>
        where TEntity : class
    {
        void Add( TEntity entity );

        void Add( IEnumerable<TEntity> entities );
    }
}