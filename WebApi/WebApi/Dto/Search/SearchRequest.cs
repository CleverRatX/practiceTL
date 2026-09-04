using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Search
{
    public class SearchRequest
    {
        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public DateOnly? ArrivalDate { get; set; }

        [Required]
        public DateOnly? DepartureDate { get; set; }

        [Range( 1, 50 )]
        public int Guests { get; set; } = 1;

        public decimal? MaxPrice { get; set; }
    }
}
