using Application.Dto;

namespace Application.Services.Search
{
    public interface IAccommodationSearchService
    {
        Task<IReadOnlyList<SearchResult>> SearchAsync( SearchCriteria criteria );
    }
}
