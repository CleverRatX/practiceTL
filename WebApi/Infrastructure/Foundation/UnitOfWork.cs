using System.Data;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Foundation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingDbContext _context;

        public UnitOfWork( BookingDbContext context )
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ITransaction> BeginTransactionAsync( IsolationLevel isolationLevel )
        {
            IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync( isolationLevel );

            return new Transaction( transaction );
        }
    }
}