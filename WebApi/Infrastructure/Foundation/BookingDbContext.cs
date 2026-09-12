using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext( DbContextOptions<BookingDbContext> options )
            : base( options )
        {
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.ApplyConfigurationsFromAssembly( typeof( BookingDbContext ).Assembly );

            base.OnModelCreating( modelBuilder );
        }
    }
}