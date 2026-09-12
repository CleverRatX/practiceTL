using System.Data;

namespace Application.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();

        Task<ITransaction> BeginTransactionAsync( IsolationLevel isolationLevel );
    }
}