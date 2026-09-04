using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Properties
{
    public class CreatePropertyRequest
    {
        [Required]
        [MaxLength( 200 )]
        public string Name { get; init; } = string.Empty;

        [Required]
        [MaxLength( 100 )]
        public string Country { get; init; } = string.Empty;

        [Required]
        [MaxLength( 100 )]
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
