namespace Domain.Repositories
{
    public interface IRemovableRepository<TEntity>
        where TEntity : class
    {
        void Remove( TEntity entity );

        void Remove( IEnumerable<TEntity> entities );
    }
}