using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApi.Dto.Properties
{
    public class UpdatePropertyRequest
    {
        [Required]
        [MaxLength( 200 )]
        [DefaultValue( "Отель" )]
        public string Name { get; init; } = string.Empty;

        [Required]
        [MaxLength( 100 )]
        [DefaultValue( "Россия" )]
        public string Country { get; init; } = string.Empty;

        [Required]
        [MaxLength( 100 )]
        [DefaultValue( "Йошкар-Ола" )]
        public string City { get; init; } = string.Empty;

        [Required]
        [MaxLength( 300 )]
        public string Address { get; init; } = string.Empty;

        [Range( -90, 90 )]
        public double Latitude { get; init; }

        [Range( -180, 180 )]
        public double Longitude { get; init; }
    }
}
