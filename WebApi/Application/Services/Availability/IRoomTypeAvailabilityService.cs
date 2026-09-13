using Domain.Entities;

namespace Application.Services.Availability
{
    public interface IRoomTypeAvailabilityService
    {
        Task<bool> HasVacantRoomsAsync( RoomType roomType, DateOnly arrivalDate, DateOnly departureDate );

        Task<IReadOnlyList<RoomType>> SelectVacantAsync(
            IReadOnlyList<RoomType> roomTypes,
            DateOnly arrivalDate,
            DateOnly departureDate );
    }
}