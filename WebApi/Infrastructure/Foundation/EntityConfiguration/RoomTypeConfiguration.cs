using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.EntityConfiguration
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        private static readonly ValueComparer<IReadOnlyList<string>> _stringListComparer = new(
            ( first, second ) => first!.SequenceEqual( second! ),
            list => list.Aggregate( 0, ( hash, item ) => HashCode.Combine( hash, item.GetHashCode() ) ),
            list => list.ToList() );

        public void Configure( EntityTypeBuilder<RoomType> builder )
        {
            builder.ToTable( "RoomTypes" );

            builder.HasKey( roomType => roomType.Id );

            builder.Property( roomType => roomType.Id )
                .ValueGeneratedNever();

            builder.Property( roomType => roomType.Name )
                .IsRequired()
                .HasMaxLength( 200 );

            builder.Property( roomType => roomType.DailyPrice )
                .HasColumnType( "decimal(18,2)" );

            builder.Property( roomType => roomType.Currency )
                .IsRequired()
                .HasMaxLength( 3 );

            builder.Property( roomType => roomType.Services )
                .HasConversion(
                    services => JsonSerializer.Serialize( services, JsonSerializerOptions.Default ),
                    json => JsonSerializer.Deserialize<List<string>>( json, JsonSerializerOptions.Default )! )
                .Metadata.SetValueComparer( _stringListComparer );

            builder.Property( roomType => roomType.Amenities )
                .HasConversion(
                    amenities => JsonSerializer.Serialize( amenities, JsonSerializerOptions.Default ),
                    json => JsonSerializer.Deserialize<List<string>>( json, JsonSerializerOptions.Default )! )
                .Metadata.SetValueComparer( _stringListComparer );

            builder.HasOne<Property>()
                .WithMany()
                .HasForeignKey( roomType => roomType.PropertyId )
                .OnDelete( DeleteBehavior.Cascade );

            builder.HasIndex( roomType => roomType.PropertyId );
        }
    }
}
