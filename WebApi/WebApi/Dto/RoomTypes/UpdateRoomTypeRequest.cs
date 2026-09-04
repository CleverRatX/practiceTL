using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApi.Dto.RoomTypes
{
    public class UpdateRoomTypeRequest
    {
        [Required]
        [MaxLength( 200 )]
        [DefaultValue( "Стандарт двухместный" )]
        public string Name { get; init; } = string.Empty;

        [Range( 0.01, 1_000_000 )]
        [DefaultValue( 4500 )]
        public decimal DailyPrice { get; init; }

        [Required]
        [StringLength( 3, MinimumLength = 3 )]
        [DefaultValue( "RUB" )]
        public string Currency { get; init; } = string.Empty;

        [Range( 1, 50 )]
        [DefaultValue( 1 )]
        public int MinPersonCount { get; init; } = 1;

        [Range( 1, 50 )]
        [DefaultValue( 2 )]
        public int MaxPersonCount { get; init; } = 1;

        [Range( 1, 10_000 )]
        [DefaultValue( 5 )]
        public int RoomCount { get; init; } = 1;

        public IReadOnlyList<string>? Services { get; init; }

        public IReadOnlyList<string>? Amenities { get; init; }
    }
}
