using Application.Services.Reservations;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mapping;
using WebApi.Models.Errors;
using WebApi.Models.Reservations;

namespace WebApi.Controllers
{
    /// <summary>
    /// Создание, просмотр и отмена бронирований.
    /// </summary>
    [ApiController]
    [Route( "api/reservations" )]
    [ApiExplorerSettings( GroupName = ApiDocuments.Reservations )]
    [Produces( "application/json" )]
    public class ReservationsController : ControllerBase
    {
        private readonly IGetReservationsService _getReservationsService;
        private readonly IGetReservationService _getReservationService;
        private readonly ICreateReservationService _createReservationService;
        private readonly ICancelReservationService _cancelReservationService;

        public ReservationsController(
            IGetReservationsService getReservationsService,
            IGetReservationService getReservationService,
            ICreateReservationService createReservationService,
            ICancelReservationService cancelReservationService )
        {
            _getReservationsService = getReservationsService;
            _getReservationService = getReservationService;
            _createReservationService = createReservationService;
            _cancelReservationService = cancelReservationService;
        }

        /// <summary>
        /// Возвращает бронирования, подходящие под условия отбора.
        /// </summary>
        /// <param name="request">Условия отбора бронирований.</param>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyList<ReservationResponse> ), StatusCodes.Status200OK )]
        public async Task<ActionResult<IReadOnlyList<ReservationResponse>>> Get(
            [FromQuery] ReservationFilterRequest request )
        {
            IReadOnlyList<Reservation> reservations = await _getReservationsService.GetAsync( request.ToFilter() );

            return Ok( reservations.ToResponse() );
        }

        /// <summary>
        /// Возвращает бронирование по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор бронирования.</param>
        [HttpGet( "{id:guid}" )]
        [ProducesResponseType( typeof( ReservationResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        public async Task<ActionResult<ReservationResponse>> GetById( [FromRoute] Guid id )
        {
            Reservation reservation = await _getReservationService.GetByIdAsync( id );

            return Ok( reservation.ToResponse() );
        }

        /// <summary>
        /// Создаёт бронирование, если на выбранные даты есть свободный номер.
        /// </summary>
        /// <param name="request">Данные нового бронирования.</param>
        [HttpPost]
        [ProducesResponseType( typeof( ReservationResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status400BadRequest )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status409Conflict )]
        public async Task<ActionResult<ReservationResponse>> Create( [FromBody] CreateReservationRequest request )
        {
            Reservation reservation = await _createReservationService.CreateAsync( request.ToDto() );

            return CreatedAtAction( nameof( GetById ), new { id = reservation.Id }, reservation.ToResponse() );
        }

        /// <summary>
        /// Отменяет бронирование.
        /// </summary>
        /// <param name="id">Идентификатор бронирования.</param>
        [HttpDelete( "{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status404NotFound )]
        [ProducesResponseType( typeof( IReadOnlyList<ErrorResponse> ), StatusCodes.Status409Conflict )]
        public async Task<IActionResult> Cancel( [FromRoute] Guid id )
        {
            await _cancelReservationService.CancelAsync( id );

            return NoContent();
        }
    }
}
