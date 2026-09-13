namespace Application.Dto
{
    public class NewReservation
    {
        public Guid PropertyId { get; set; }

        public Guid RoomTypeId { get; set; }

        public DateOnly ArrivalDate { get; set; }

        public DateOnly DepartureDate { get; set; }

        public string GuestName { get; set; } = string.Empty;

        public string GuestPhoneNumber { get; set; } = string.Empty;

        public int GuestCount { get; set; }
    }
}
