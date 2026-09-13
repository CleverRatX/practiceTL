using Domain.Exceptions;
using Domain.Validation;

namespace Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; private set; }

        public Guid PropertyId { get; private set; }

        public Guid RoomTypeId { get; private set; }

        public DateOnly ArrivalDate { get; private set; }

        public DateOnly DepartureDate { get; private set; }

        public TimeOnly ArrivalTime { get; private set; }

        public TimeOnly DepartureTime { get; private set; }

        public string GuestName { get; private set; } = string.Empty;

        public string GuestPhoneNumber { get; private set; } = string.Empty;

        public int GuestCount { get; private set; }

        public decimal Total { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        public ReservationStatus Status { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset? CancelledAt { get; private set; }

        private Reservation()
        {
        }

        public Reservation(
            Guid propertyId,
            Guid roomTypeId,
            DateOnly arrivalDate,
            DateOnly departureDate,
            TimeOnly arrivalTime,
            TimeOnly departureTime,
            string guestName,
            string guestPhoneNumber,
            int guestCount,
            decimal total,
            string currency,
            DateTimeOffset createdAt )
        {
            if ( departureDate <= arrivalDate )
            {
                throw new DomainValidationException( "Дата выезда должна быть позже даты заезда." );
            }

            Id = Guid.NewGuid();
            PropertyId = propertyId;
            RoomTypeId = roomTypeId;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
            GuestName = Validator.NormalizedText( guestName, "ФИО гостя не может быть пустым." );
            GuestPhoneNumber = Validator.NormalizedText( guestPhoneNumber, "Телефон гостя не может быть пустым." );
            GuestCount = Validator.PositiveCount( guestCount, "Количество гостей должно быть больше нуля." );
            Total = Validator.NotNegative( total, "Стоимость бронирования не может быть отрицательной." );
            Currency = Validator.NormalizedCurrencyCode( currency );
            Status = ReservationStatus.Active;
            CreatedAt = createdAt;
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
