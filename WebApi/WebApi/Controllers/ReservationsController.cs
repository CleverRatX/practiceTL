using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto.Reservations;
using WebApi.Mapping;

namespace WebApi.Controllers
{
    [ApiController]
    [Route( "api/reservations" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Reservations )]
    [Produces( "application/json" )]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController( IReservationService reservationService )
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyList<ReservationResponse> ), StatusCodes.Status200OK )]
        public async Task<ActionResult<IReadOnlyList<ReservationResponse>>> Get(
            [FromQuery] ReservationFilterRequest request,
            CancellationToken cancellationToken )
        {
            IReadOnlyList<Reservation> reservations = await _reservationService.GetAsync( request.ToFilter(), cancellationToken );

            return Ok( reservations.ToResponse() );
        }

        [HttpGet( "{id:guid}" )]
        [ProducesResponseType( typeof( ReservationResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<ReservationResponse>> GetById( [FromRoute] Guid id, CancellationToken cancellationToken )
        {
            Reservation reservation = await _reservationService.GetByIdAsync( id, cancellationToken );

            return Ok( reservation.ToResponse() );
        }

        [HttpPost]
        [ProducesResponseType( typeof( ReservationResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status409Conflict )]
        public async Task<ActionResult<ReservationResponse>> Create(
            [FromBody] CreateReservationRequest request,
            CancellationToken cancellationToken )
        {
            Reservation reservation = await _reservationService.CreateAsync( request.ToDomain(), cancellationToken );

            return CreatedAtAction( nameof( GetById ), new { id = reservation.Id }, reservation.ToResponse() );
        }

        [HttpDelete( "{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status404NotFound )]
        [ProducesResponseType( typeof( ProblemDetails ), StatusCodes.Status409Conflict )]
        public async Task<IActionResult> Cancel( [FromRoute] Guid id, CancellationToken cancellationToken )
        {
            await _reservationService.CancelAsync( id, cancellationToken );

            return NoContent();
        }
    }
}
