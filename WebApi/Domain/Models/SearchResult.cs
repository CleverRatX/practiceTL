using Domain.Entities;

namespace Domain.Models
{
    public record SearchResult(
        Property Property,
        RoomType RoomType,
        int Nights,
        decimal Total );
}
