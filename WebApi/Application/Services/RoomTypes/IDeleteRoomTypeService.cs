namespace Application.Services.RoomTypes
{
    public interface IDeleteRoomTypeService
    {
        Task DeleteAsync( Guid id );
    }
}
