namespace WebApi.Dto.Search
{
    public record SearchOptionResponse(
        Guid PropertyId,
        string PropertyName,
        string Country,
        string City,
        string Address,
        Guid RoomTypeId,
        string RoomTypeName,
        decimal DailyPrice,
        int MinPersonCount,
        int MaxPersonCount,
        IReadOnlyList<string> Services,
        IReadOnlyList<string> Amenities,
        int Nights,
        decimal Total,
        string Currency );
}
