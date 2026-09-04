using Domain.Entities;

namespace Domain.Services
{
    public interface IAvailabilityService
    {
        Task<bool> IsAvailableAsync(
            RoomType roomType,
            DateOnly arrivalDate,
            DateOnly departureDate,
            CancellationToken cancellationToken );
    }
}
