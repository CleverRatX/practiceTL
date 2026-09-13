using Application.Dto;
using Application.Services.Search;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mapping;
using WebApi.Models.Errors;
using WebApi.Models.Search;

namespace WebApi.Controllers
{
    /// <summary>
    /// Поиск свободных вариантов размещения.
    /// </summary>
    [ApiController]
    [Route( "api/search" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Reservations )]
    [Produces( "application/json" )]
    public class SearchController : ControllerBase
    {
        private readonly IAccommodationSearchService _accommodationSearchService;

        public SearchController( IAccommodationSearchService accommodationSearchService )
        {
            _accommodationSearchService = accommodationSearchService;
        }

        /// <summary>
        /// Возвращает свободные категории номеров, подходящие под условия поиска.
        /// </summary>
        /// <param name="request">Условия поиска.</param>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyList<SearchOptionResponse> ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status400BadRequest )]
        public async Task<ActionResult<IReadOnlyList<SearchOptionResponse>>> Search( [FromQuery] SearchRequest request )
        {
            IReadOnlyList<SearchResult> options = await _accommodationSearchService.SearchAsync( request.ToCriteria() );

            return Ok( options.ToResponse() );
        }
    }
}
