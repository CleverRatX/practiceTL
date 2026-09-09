using Application.Services.RoomTypes;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mapping;
using WebApi.Models.Errors;
using WebApi.Models.RoomTypes;

namespace WebApi.Controllers
{
    /// <summary>
    /// Категории номеров в разрезе средства размещения.
    /// </summary>
    [ApiController]
    [Route( "api/properties/{propertyId:guid}/roomtypes" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Properties )]
    [Produces( "application/json" )]
    public class PropertyRoomTypesController : ControllerBase
    {
        private readonly IGetPropertyRoomTypesService _getPropertyRoomTypesService;
        private readonly ICreateRoomTypeService _createRoomTypeService;

        public PropertyRoomTypesController(
            IGetPropertyRoomTypesService getPropertyRoomTypesService,
            ICreateRoomTypeService createRoomTypeService )
        {
            _getPropertyRoomTypesService = getPropertyRoomTypesService;
            _createRoomTypeService = createRoomTypeService;
        }

        /// <summary>
        /// Возвращает категории номеров средства размещения.
        /// </summary>
        /// <param name="propertyId">Идентификатор средства размещения.</param>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyList<RoomTypeResponse> ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<IReadOnlyList<RoomTypeResponse>>> GetByPropertyId( [FromRoute] Guid propertyId )
        {
            IReadOnlyList<RoomType> roomTypes = await _getPropertyRoomTypesService.GetByPropertyIdAsync( propertyId );

            return Ok( roomTypes.ToResponse() );
        }

        /// <summary>
        /// Добавляет категорию номера в средство размещения.
        /// </summary>
        /// <param name="propertyId">Идентификатор средства размещения.</param>
        /// <param name="request">Данные новой категории номера.</param>
        [HttpPost]
        [ProducesResponseType( typeof( RoomTypeResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<RoomTypeResponse>> Create(
            [FromRoute] Guid propertyId,
            [FromBody] CreateRoomTypeRequest request )
        {
            RoomType roomType = await _createRoomTypeService.CreateAsync( propertyId, request.ToData() );

            return CreatedAtAction(
                nameof( RoomTypesController.GetById ),
                "RoomTypes",
                new { id = roomType.Id },
                roomType.ToResponse() );
        }
    }
}
