using Application.Services.RoomTypes;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mapping;
using WebApi.Models.Errors;
using WebApi.Models.RoomTypes;

namespace WebApi.Controllers
{
    /// <summary>
    /// Работа с отдельной категорией номера.
    /// </summary>
    [ApiController]
    [Route( "api/roomtypes" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Properties )]
    [Produces( "application/json" )]
    public class RoomTypesController : ControllerBase
    {
        private readonly IGetRoomTypeService _getRoomTypeService;
        private readonly IUpdateRoomTypeService _updateRoomTypeService;
        private readonly IDeleteRoomTypeService _deleteRoomTypeService;

        public RoomTypesController(
            IGetRoomTypeService getRoomTypeService,
            IUpdateRoomTypeService updateRoomTypeService,
            IDeleteRoomTypeService deleteRoomTypeService )
        {
            _getRoomTypeService = getRoomTypeService;
            _updateRoomTypeService = updateRoomTypeService;
            _deleteRoomTypeService = deleteRoomTypeService;
        }

        /// <summary>
        /// Возвращает категорию номера по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории номера.</param>
        [HttpGet( "{id:guid}" )]
        [ProducesResponseType( typeof( RoomTypeResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<RoomTypeResponse>> GetById( [FromRoute] Guid id )
        {
            RoomType roomType = await _getRoomTypeService.GetByIdAsync( id );

            return Ok( roomType.ToResponse() );
        }

        /// <summary>
        /// Обновляет категорию номера.
        /// </summary>
        /// <param name="id">Идентификатор категории номера.</param>
        /// <param name="request">Новые данные категории номера.</param>
        [HttpPut( "{id:guid}" )]
        [ProducesResponseType( typeof( RoomTypeResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<RoomTypeResponse>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateRoomTypeRequest request )
        {
            RoomType roomType = await _updateRoomTypeService.UpdateAsync( id, request.ToData() );

            return Ok( roomType.ToResponse() );
        }

        /// <summary>
        /// Удаляет категорию номера.
        /// </summary>
        /// <param name="id">Идентификатор категории номера.</param>
        [HttpDelete( "{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status409Conflict )]
        public async Task<IActionResult> Delete( [FromRoute] Guid id )
        {
            await _deleteRoomTypeService.DeleteAsync( id );

            return NoContent();
        }
    }
}
