namespace Application.Repositories
{
    public interface ITransaction : IAsyncDisposable
    {
        Task CommitAsync();
    }
}