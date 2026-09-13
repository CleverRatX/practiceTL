using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.EntityConfiguration
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure( EntityTypeBuilder<Property> builder )
        {
            builder.ToTable( "Properties" );

            builder.HasKey( property => property.Id );

            builder.Property( property => property.Id )
                .ValueGeneratedNever();

            builder.Property( property => property.Name )
                .IsRequired()
                .HasMaxLength( 200 );

            builder.Property( property => property.Country )
                .IsRequired()
                .HasMaxLength( 100 );

            builder.Property( property => property.City )
                .IsRequired()
                .HasMaxLength( 100 );

            builder.Property( property => property.Address )
                .IsRequired()
                .HasMaxLength( 300 );

            builder.HasIndex( property => property.City );
        }
    }
}
