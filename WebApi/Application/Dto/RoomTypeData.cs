namespace Application.Dto
{
    public class RoomTypeData
    {
        public string Name { get; set; } = string.Empty;

        public decimal DailyPrice { get; set; }

        public string Currency { get; set; } = string.Empty;

        public int MinPersonCount { get; set; }

        public int MaxPersonCount { get; set; }

        public int RoomCount { get; set; }

        public IReadOnlyList<string> Services { get; set; } = [];

        public IReadOnlyList<string> Amenities { get; set; } = [];
    }
}
