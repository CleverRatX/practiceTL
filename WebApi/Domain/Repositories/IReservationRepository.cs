using Domain.Entities;

namespace Domain.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation?> GetByIdAsync( Guid id );

        Task<IReadOnlyList<Reservation>> GetAsync( ReservationFilter filter );

        Task<IReadOnlyList<Reservation>> GetOverlappingAsync(
            Guid roomTypeId,
            DateOnly arrivalDate,
            DateOnly departureDate );

        Task<IReadOnlyList<Reservation>> GetOverlappingAsync(
            IReadOnlyList<Guid> roomTypeIds,
            DateOnly arrivalDate,
            DateOnly departureDate );

        Task<IReadOnlyList<Reservation>> GetActiveByRoomTypeIdAsync( Guid roomTypeId, DateOnly fromDate );

        Task<bool> HasActiveByPropertyIdAsync( Guid propertyId );

        Task<bool> HasActiveByRoomTypeIdAsync( Guid roomTypeId );
    }
}