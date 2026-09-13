using Application.Services.Properties;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mapping;
using WebApi.Models.Errors;
using WebApi.Models.Properties;

namespace WebApi.Controllers
{
    /// <summary>
    /// Управление средствами размещения.
    /// </summary>
    [ApiController]
    [Route( "api/properties" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Properties )]
    [Produces( "application/json" )]
    public class PropertiesController : ControllerBase
    {
        private readonly IGetPropertiesService _getPropertiesService;
        private readonly IGetPropertyService _getPropertyService;
        private readonly ICreatePropertyService _createPropertyService;
        private readonly IUpdatePropertyService _updatePropertyService;
        private readonly IDeletePropertyService _deletePropertyService;

        public PropertiesController(
            IGetPropertiesService getPropertiesService,
            IGetPropertyService getPropertyService,
            ICreatePropertyService createPropertyService,
            IUpdatePropertyService updatePropertyService,
            IDeletePropertyService deletePropertyService )
        {
            _getPropertiesService = getPropertiesService;
            _getPropertyService = getPropertyService;
            _createPropertyService = createPropertyService;
            _updatePropertyService = updatePropertyService;
            _deletePropertyService = deletePropertyService;
        }

        /// <summary>
        /// Возвращает все средства размещения.
        /// </summary>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyList<PropertyResponse> ), StatusCodes.Status200OK )]
        public async Task<ActionResult<IReadOnlyList<PropertyResponse>>> GetAll()
        {
            IReadOnlyList<Property> properties = await _getPropertiesService.GetAllAsync();

            return Ok( properties.ToResponse() );
        }

        /// <summary>
        /// Возвращает средство размещения по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор средства размещения.</param>
        [HttpGet( "{id:guid}" )]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<PropertyResponse>> GetById( [FromRoute] Guid id )
        {
            Property property = await _getPropertyService.GetByIdAsync( id );

            return Ok( property.ToResponse() );
        }

        /// <summary>
        /// Создаёт средство размещения.
        /// </summary>
        /// <param name="request">Данные нового средства размещения.</param>
        [HttpPost]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status400BadRequest )]
        public async Task<ActionResult<PropertyResponse>> Create( [FromBody] CreatePropertyRequest request )
        {
            Property property = await _createPropertyService.CreateAsync( request.ToData() );

            return CreatedAtAction( nameof( GetById ), new { id = property.Id }, property.ToResponse() );
        }

        /// <summary>
        /// Обновляет средство размещения.
        /// </summary>
        /// <param name="id">Идентификатор средства размещения.</param>
        /// <param name="request">Новые данные средства размещения.</param>
        [HttpPut( "{id:guid}" )]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<PropertyResponse>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdatePropertyRequest request )
        {
            Property property = await _updatePropertyService.UpdateAsync( id, request.ToData() );

            return Ok( property.ToResponse() );
        }

        /// <summary>
        /// Удаляет средство размещения вместе с его категориями номеров.
        /// </summary>
        /// <param name="id">Идентификатор средства размещения.</param>
        [HttpDelete( "{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status409Conflict )]
        public async Task<IActionResult> Delete( [FromRoute] Guid id )
        {
            await _deletePropertyService.DeleteAsync( id );

            return NoContent();
        }
    }
}
