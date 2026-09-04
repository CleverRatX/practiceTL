using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto.Properties;
using WebApi.Mapping;

namespace WebApi.Controllers
{
    [ApiController]
    [Route( "api/properties" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Properties )]
    [Produces( "application/json" )]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController( IPropertyService propertyService )
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyList<PropertyResponse> ), StatusCodes.Status200OK )]
        public async Task<ActionResult<IReadOnlyList<PropertyResponse>>> GetAll( CancellationToken cancellationToken )
        {
            IReadOnlyList<Property> properties = await _propertyService.GetAllAsync( cancellationToken );

            return Ok( properties.ToResponse() );
        }

        [HttpGet( "{id:guid}" )]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<PropertyResponse>> GetById( [FromRoute] Guid id, CancellationToken cancellationToken )
        {
            Property property = await _propertyService.GetByIdAsync( id, cancellationToken );

            return Ok( property.ToResponse() );
        }

        [HttpPost]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status400BadRequest )]
        public async Task<ActionResult<PropertyResponse>> Create(
            [FromBody] CreatePropertyRequest request,
            CancellationToken cancellationToken )
        {
            Property property = await _propertyService.CreateAsync( request.ToData(), cancellationToken );

            return CreatedAtAction( nameof( GetById ), new { id = property.Id }, property.ToResponse() );
        }

        [HttpPut( "{id:guid}" )]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<PropertyResponse>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdatePropertyRequest request,
            CancellationToken cancellationToken )
        {
            Property property = await _propertyService.UpdateAsync( id, request.ToData(), cancellationToken );

            return Ok( property.ToResponse() );
        }

        [HttpDelete( "{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status409Conflict )]
        public async Task<IActionResult> Delete( [FromRoute] Guid id, CancellationToken cancellationToken )
        {
            await _propertyService.DeleteAsync( id, cancellationToken );

            return NoContent();
        }
    }
}
