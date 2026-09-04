namespace WebApi.Dto.RoomTypes
{
    public record RoomTypeResponse(
        Guid Id,
        Guid PropertyId,
        string Name,
        decimal DailyPrice,
        string Currency,
        int MinPersonCount,
        int MaxPersonCount,
        int RoomCount,
        IReadOnlyList<string> Services,
        IReadOnlyList<string> Amenities );
}
