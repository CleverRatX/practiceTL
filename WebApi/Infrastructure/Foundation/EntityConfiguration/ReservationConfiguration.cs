using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.EntityConfiguration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure( EntityTypeBuilder<Reservation> builder )
        {
            builder.ToTable( "Reservations" );

            builder.HasKey( reservation => reservation.Id );

            builder.Property( reservation => reservation.Id )
                .ValueGeneratedNever();

            builder.Property( reservation => reservation.GuestName )
                .IsRequired()
                .HasMaxLength( 200 );

            builder.Property( reservation => reservation.GuestPhoneNumber )
                .IsRequired()
                .HasMaxLength( 30 );

            builder.Property( reservation => reservation.Total )
                .HasColumnType( "decimal(18,2)" );

            builder.Property( reservation => reservation.Currency )
                .IsRequired()
                .HasMaxLength( 3 );

            builder.Property( reservation => reservation.Status )
                .IsRequired();

            builder.HasOne<RoomType>()
                .WithMany()
                .HasForeignKey( reservation => reservation.RoomTypeId )
                .OnDelete( DeleteBehavior.Cascade );

            builder.HasOne<Property>()
                .WithMany()
                .HasForeignKey( reservation => reservation.PropertyId )
                .OnDelete( DeleteBehavior.NoAction );

            builder.HasIndex( reservation => reservation.PropertyId );

            builder.HasIndex( reservation => new
            {
                reservation.RoomTypeId,
                reservation.ArrivalDate,
                reservation.DepartureDate
            } );
        }
    }
}
