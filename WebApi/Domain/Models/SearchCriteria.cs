namespace Domain.Models
{
    public record SearchCriteria(
        string City,
        DateOnly ArrivalDate,
        DateOnly DepartureDate,
        int GuestCount,
        decimal? MaxDailyPrice = null );
}