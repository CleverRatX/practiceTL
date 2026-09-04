namespace WebApi.Dto.Reservations
{
    public class ReservationFilterRequest
    {
        public Guid? PropertyId { get; set; }

        public Guid? RoomTypeId { get; set; }

        public string? GuestName { get; set; }

        public DateOnly? ArrivalDateFrom { get; set; }

        public DateOnly? DepartureDateTo { get; set; }

        public bool IncludeCancelled { get; set; }
    }
}
