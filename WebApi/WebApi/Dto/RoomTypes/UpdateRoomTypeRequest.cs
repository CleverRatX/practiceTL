using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.RoomTypes
{
    public class UpdateRoomTypeRequest
    {
        [Required]
        [MaxLength( 200 )]
        public string Name { get; init; } = string.Empty;

        [Range( 0.01, 1_000_000 )]
        public decimal DailyPrice { get; init; }

        [Required]
        [StringLength( 3, MinimumLength = 3 )]
        public string Currency { get; init; } = string.Empty;

        [Range( 1, 50 )]
        public int MinPersonCount { get; init; } = 1;

        [Range( 1, 50 )]
        public int MaxPersonCount { get; init; } = 1;

        [Range( 1, 10_000 )]
        public int RoomCount { get; init; } = 1;

        public IReadOnlyList<string>? Services { get; init; }

        public IReadOnlyList<string>? Amenities { get; init; }
    }
}
