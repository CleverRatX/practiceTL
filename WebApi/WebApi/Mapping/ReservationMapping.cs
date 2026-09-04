using Domain.Entities;
using Domain.Models;
using WebApi.Dto.Reservations;

namespace WebApi.Mapping
{
    public static class ReservationMapping
    {
        private static readonly TimeOnly _defaultArrivalTime = new( 14, 0 );
        private static readonly TimeOnly _defaultDepartureTime = new( 12, 0 );

        public static ReservationResponse ToResponse( this Reservation reservation )
        {
            return new ReservationResponse(
                reservation.Id,
                reservation.PropertyId,
                reservation.RoomTypeId,
                reservation.ArrivalDate,
                reservation.DepartureDate,
                reservation.ArrivalTime,
                reservation.DepartureTime,
                reservation.GuestName,
                reservation.GuestPhoneNumber,
                reservation.GuestCount,
                reservation.Nights,
                reservation.Total,
                reservation.Currency,
                reservation.Status,
                reservation.CreatedAt,
                reservation.CancelledAt );
        }

        public static IReadOnlyList<ReservationResponse> ToResponse( this IReadOnlyList<Reservation> reservations )
        {
            return reservations
                .Select( reservation => reservation.ToResponse() )
                .ToList();
        }

        public static NewReservation ToDomain( this CreateReservationRequest request )
        {
            return new NewReservation(
                request.PropertyId!.Value,
                request.RoomTypeId!.Value,
                request.ArrivalDate!.Value,
                request.DepartureDate!.Value,
                request.ArrivalTime ?? _defaultArrivalTime,
                request.DepartureTime ?? _defaultDepartureTime,
                request.GuestName,
                request.GuestPhoneNumber,
                request.Guests );
        }

        public static ReservationFilter ToFilter( this ReservationFilterRequest request )
        {
            return new ReservationFilter(
                request.PropertyId,
                request.RoomTypeId,
                request.GuestName,
                request.ArrivalDateFrom,
                request.DepartureDateTo,
                request.IncludeCancelled );
        }
    }
}
