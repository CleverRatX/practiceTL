using Domain.Exceptions;
using Domain.Models;
using Domain.Validation;

namespace Domain.Entities
{
    public class Reservation
    {
        public Reservation( NewReservation request, decimal total, string currency, DateTimeOffset createdAt )
        {
            if ( request.DepartureDate <= request.ArrivalDate )
            {
                throw new DomainValidationException( "Дата выезда должна быть позже даты заезда." );
            }

            Id = Guid.NewGuid();
            PropertyId = request.PropertyId;
            RoomTypeId = request.RoomTypeId;
            ArrivalDate = request.ArrivalDate;
            DepartureDate = request.DepartureDate;
            ArrivalTime = request.ArrivalTime;
            DepartureTime = request.DepartureTime;
            GuestName = Validated.Text( request.GuestName, "ФИО гостя не может быть пустым." );
            GuestPhoneNumber = Validated.Text( request.GuestPhoneNumber, "Телефон гостя не может быть пустым." );
            GuestCount = Validated.PositiveCount( request.GuestCount, "Количество гостей должно быть больше нуля." );
            Total = Validated.NotNegativeAmount( total, "Стоимость бронирования не может быть отрицательной." );
            Currency = Validated.Currency( currency );
            Status = ReservationStatus.Active;
            CreatedAt = createdAt;
        }

        public Guid Id { get; }

        public Guid PropertyId { get; }

        public Guid RoomTypeId { get; }

        public DateOnly ArrivalDate { get; }

        public DateOnly DepartureDate { get; }

        public TimeOnly ArrivalTime { get; }

        public TimeOnly DepartureTime { get; }

        public string GuestName { get; }

        public string GuestPhoneNumber { get; }

        public int GuestCount { get; }

        public decimal Total { get; }

        public string Currency { get; }

        public ReservationStatus Status { get; private set; }

        public DateTimeOffset CreatedAt { get; }

        public DateTimeOffset? CancelledAt { get; private set; }

        public int Nights => DepartureDate.DayNumber - ArrivalDate.DayNumber;

        public bool IsActive => Status == ReservationStatus.Active;

        public bool CoversNight( DateOnly night )
        {
            return night >= ArrivalDate && night < DepartureDate;
        }

        public void Cancel( DateTimeOffset cancelledAt )
        {
            if ( Status == ReservationStatus.Cancelled )
            {
                throw new ConflictException( "Бронирование уже отменено." );
            }

            Status = ReservationStatus.Cancelled;
            CancelledAt = cancelledAt;
        }
    }
}
