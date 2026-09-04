namespace Domain.Models
{
    public record RoomTypeData(
        string Name,
        decimal DailyPrice,
        string Currency,
        int MinPersonCount,
        int MaxPersonCount,
        int RoomCount,
        IReadOnlyList<string> Services,
        IReadOnlyList<string> Amenities );
}
