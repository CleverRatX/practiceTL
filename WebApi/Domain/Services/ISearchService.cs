using Domain.Models;

namespace Domain.Services
{
    public interface ISearchService
    {
        Task<IReadOnlyList<SearchResult>> SearchAsync( SearchCriteria criteria, CancellationToken cancellationToken );
    }
}
