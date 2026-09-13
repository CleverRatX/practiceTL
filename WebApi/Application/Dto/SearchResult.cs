using Domain.Entities;

namespace Application.Dto
{
    public class SearchResult
    {
        public Property Property { get; set; } = null!;

        public RoomType RoomType { get; set; } = null!;

        public int Nights { get; set; }

        public decimal Total { get; set; }
    }
}
