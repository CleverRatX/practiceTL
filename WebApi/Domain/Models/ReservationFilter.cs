namespace Domain.Models
{
    public record ReservationFilter(
        Guid? PropertyId = null,
        Guid? RoomTypeId = null,
        string? GuestName = null,
        DateOnly? ArrivalDateFrom = null,
        DateOnly? DepartureDateTo = null,
        bool IncludeCancelled = false );
}
