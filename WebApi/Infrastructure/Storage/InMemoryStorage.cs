using System.Collections.Concurrent;
using Domain.Entities;

namespace Infrastructure.Storage
{
    public class InMemoryStorage
    {
        public ConcurrentDictionary<Guid, Property> Properties { get; } = new();

        public ConcurrentDictionary<Guid, RoomType> RoomTypes { get; } = new();

        public ConcurrentDictionary<Guid, Reservation> Reservations { get; } = new();
    }
}
