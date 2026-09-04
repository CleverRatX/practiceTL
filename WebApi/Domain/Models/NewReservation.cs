namespace Domain.Models
{
    public record NewReservation(
        Guid PropertyId,
        Guid RoomTypeId,
        DateOnly ArrivalDate,
        DateOnly DepartureDate,
        TimeOnly ArrivalTime,
        TimeOnly DepartureTime,
        string GuestName,
        string GuestPhoneNumber,
        int GuestCount );
}
