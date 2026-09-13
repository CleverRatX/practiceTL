using System.Data;

namespace Application.Persistence
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();

        Task<ITransaction> BeginTransactionAsync( IsolationLevel isolationLevel );
    }
}