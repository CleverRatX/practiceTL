using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Property> Properties => Set<Property>();

        public DbSet<RoomType> RoomTypes => Set<RoomType>();

        public DbSet<Reservation> Reservations => Set<Reservation>();

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
