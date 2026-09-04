using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto.Search;
using WebApi.Mapping;

namespace WebApi.Controllers
{
    [ApiController]
    [Route( "api/search" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Reservations )]
    [Produces( "application/json" )]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController( ISearchService searchService )
        {
            _searchService = searchService;
        }

        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyList<SearchOptionResponse> ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status400BadRequest )]
        public async Task<ActionResult<IReadOnlyList<SearchOptionResponse>>> Search(
            [FromQuery] SearchRequest request,
            CancellationToken cancellationToken )
        {
            IReadOnlyList<SearchResult> options = await _searchService.SearchAsync( request.ToCriteria(), cancellationToken );

            return Ok( options.ToResponse() );
        }
    }
}
