using Domain.Repositories;
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

        public async Task<ITransaction> BeginTransactionAsync()
        {
            IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            return new Transaction( transaction );
        }
    }
}