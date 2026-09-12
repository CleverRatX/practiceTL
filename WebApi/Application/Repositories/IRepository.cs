namespace Application.Repositories
{
    public interface IRepository<TEntity> : IAddedRepository<TEntity>, IRemovableRepository<TEntity>
        where TEntity : class
    {
    }
}