using Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Foundation
{
    public class Transaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;

        public Transaction( IDbContextTransaction transaction )
        {
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _transaction.DisposeAsync();
        }
    }
}