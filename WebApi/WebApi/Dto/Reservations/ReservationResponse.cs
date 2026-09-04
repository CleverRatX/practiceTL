using Domain.Entities;

namespace WebApi.Dto.Reservations
{
    public record ReservationResponse(
        Guid Id,
        Guid PropertyId,
        Guid RoomTypeId,
        DateOnly ArrivalDate,
        DateOnly DepartureDate,
        TimeOnly ArrivalTime,
        TimeOnly DepartureTime,
        string GuestName,
        string GuestPhoneNumber,
        int GuestCount,
        int Nights,
        decimal Total,
        string Currency,
        ReservationStatus Status,
        DateTimeOffset CreatedAt,
        DateTimeOffset? CancelledAt );
}
