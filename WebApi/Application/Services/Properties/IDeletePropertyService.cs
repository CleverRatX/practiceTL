namespace Application.Services.Properties
{
    public interface IDeletePropertyService
    {
        Task DeleteAsync( Guid id );
    }
}
