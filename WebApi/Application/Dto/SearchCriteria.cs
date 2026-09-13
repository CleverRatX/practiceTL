namespace Application.Dto
{
    public class SearchCriteria
    {
        public string City { get; set; } = string.Empty;

        public DateOnly ArrivalDate { get; set; }

        public DateOnly DepartureDate { get; set; }

        public int GuestCount { get; set; }

        public decimal? MaxDailyPrice { get; set; }
    }
}
