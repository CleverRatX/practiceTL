namespace Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();

        Task<ITransaction> BeginTransactionAsync();
    }
}