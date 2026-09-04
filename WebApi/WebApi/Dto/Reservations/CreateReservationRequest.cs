using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
        [DefaultValue( "Иванов Иван Иванович" )]
        public string GuestName { get; init; } = string.Empty;

        [Required]
        [Phone]
        [DefaultValue( "+79123456789" )]
        public string GuestPhoneNumber { get; init; } = string.Empty;

        [Range( 1, 50 )]
        [DefaultValue( 1 )]
        public int Guests { get; init; } = 1;
    }
}
