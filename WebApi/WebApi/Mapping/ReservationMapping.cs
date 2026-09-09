using Application.Dto;
using Application.Rules;
using Domain.Entities;
using Domain.Repositories;
using WebApi.Models.Reservations;

namespace WebApi.Mapping
{
    public static class ReservationMapping
    {
        public static ReservationResponse ToResponse( this Reservation reservation )
        {
            return new ReservationResponse
            {
                Id = reservation.Id,
                PropertyId = reservation.PropertyId,
                RoomTypeId = reservation.RoomTypeId,
                ArrivalDate = reservation.ArrivalDate,
                DepartureDate = reservation.DepartureDate,
                ArrivalTime = reservation.ArrivalTime,
                DepartureTime = reservation.DepartureTime,
                GuestName = reservation.GuestName,
                GuestPhoneNumber = reservation.GuestPhoneNumber,
                GuestCount = reservation.GuestCount,
                Nights = BookingRules.CountNights( reservation.ArrivalDate, reservation.DepartureDate ),
                Total = reservation.Total,
                Currency = reservation.Currency,
                Status = reservation.Status,
                CreatedAt = reservation.CreatedAt,
                CancelledAt = reservation.CancelledAt
            };
        }

        public static IReadOnlyList<ReservationResponse> ToResponse( this IReadOnlyList<Reservation> reservations )
        {
            return reservations
                .Select( ToResponse )
                .ToList();
        }

        public static NewReservation ToDto( this CreateReservationRequest request )
        {
            return new NewReservation
            {
                PropertyId = request.PropertyId!.Value,
                RoomTypeId = request.RoomTypeId!.Value,
                ArrivalDate = request.ArrivalDate!.Value,
                DepartureDate = request.DepartureDate!.Value,
                GuestName = request.GuestName,
                GuestPhoneNumber = request.GuestPhoneNumber,
                GuestCount = request.GuestCount
            };
        }

        public static ReservationFilter ToFilter( this ReservationFilterRequest request )
        {
            return new ReservationFilter
            {
                PropertyId = request.PropertyId,
                RoomTypeId = request.RoomTypeId,
                GuestName = request.GuestName,
                ArrivalDateFrom = request.ArrivalDateFrom,
                DepartureDateTo = request.DepartureDateTo,
                IncludeCancelled = request.IncludeCancelled
            };
        }
    }
}
