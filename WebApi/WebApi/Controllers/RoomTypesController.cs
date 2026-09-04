using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto.RoomTypes;
using WebApi.Mapping;

namespace WebApi.Controllers
{
    [ApiController]
    [Route( "api" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Properties )]
    [Produces( "application/json" )]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypesController( IRoomTypeService roomTypeService )
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet( "properties/{propertyId:guid}/roomtypes" )]
        [ProducesResponseType( typeof( IReadOnlyList<RoomTypeResponse> ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<IReadOnlyList<RoomTypeResponse>>> GetByProperty(
            [FromRoute] Guid propertyId,
            CancellationToken cancellationToken )
        {
            IReadOnlyList<RoomType> roomTypes = await _roomTypeService.GetByPropertyAsync( propertyId, cancellationToken );

            return Ok( roomTypes.ToResponse() );
        }

        [HttpGet( "roomtypes/{id:guid}" )]
        [ProducesResponseType( typeof( RoomTypeResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<RoomTypeResponse>> GetById( [FromRoute] Guid id, CancellationToken cancellationToken )
        {
            RoomType roomType = await _roomTypeService.GetByIdAsync( id, cancellationToken );

            return Ok( roomType.ToResponse() );
        }

        [HttpPost( "properties/{propertyId:guid}/roomtypes" )]
        [ProducesResponseType( typeof( RoomTypeResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<RoomTypeResponse>> Create(
            [FromRoute] Guid propertyId,
            [FromBody] CreateRoomTypeRequest request,
            CancellationToken cancellationToken )
        {
            RoomType roomType = await _roomTypeService.CreateAsync( propertyId, request.ToData(), cancellationToken );

            return CreatedAtAction( nameof( GetById ), new { id = roomType.Id }, roomType.ToResponse() );
        }

        [HttpPut( "roomtypes/{id:guid}" )]
        [ProducesResponseType( typeof( RoomTypeResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<RoomTypeResponse>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateRoomTypeRequest request,
            CancellationToken cancellationToken )
        {
            RoomType roomType = await _roomTypeService.UpdateAsync( id, request.ToData(), cancellationToken );

            return Ok( roomType.ToResponse() );
        }

        [HttpDelete( "roomtypes/{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status409Conflict )]
        public async Task<IActionResult> Delete( [FromRoute] Guid id, CancellationToken cancellationToken )
        {
            await _roomTypeService.DeleteAsync( id, cancellationToken );

            return NoContent();
        }
    }
}
