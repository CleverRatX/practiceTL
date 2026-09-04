using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Reservations
{
    public class CreateReservationRequest
    {
        [Required]
        public Guid? PropertyId { get; init; }

        [Required]
        public Guid? RoomTypeId { get; init; }

        [Required]
        public DateOnly? ArrivalDate { get; init; }

        [Required]
        public DateOnly? DepartureDate { get; init; }

        public TimeOnly? ArrivalTime { get; init; }

        public TimeOnly? DepartureTime { get; init; }

        [Required]
        [MaxLength( 200 )]
        public string GuestName { get; init; } = string.Empty;

        [Required]
        [Phone]
        public string GuestPhoneNumber { get; init; } = string.Empty;

        [Range( 1, 50 )]
        public int Guests { get; init; } = 1;
    }
}
