namespace WebApi.Dto.Properties
{
    public record PropertyResponse(
        Guid Id,
        string Name,
        string Country,
        string City,
        string Address,
        double Latitude,
        double Longitude );
}
