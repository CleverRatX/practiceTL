namespace Application.Services.Reservations
{
    public interface ICancelReservationService
    {
        Task CancelAsync( Guid id );
    }
}
