using Domain.Entities;

namespace WebApi.Models.Reservations
{
    /// <summary>
    /// Бронирование.
    /// </summary>
    public class ReservationResponse
    {
        /// <summary>
        /// Идентификатор бронирования.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор средства размещения.
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Идентификатор забронированной категории номера.
        /// </summary>
        public Guid RoomTypeId { get; set; }

        /// <summary>
        /// Дата заезда.
        /// </summary>
        public DateOnly ArrivalDate { get; set; }

        /// <summary>
        /// Дата выезда.
        /// </summary>
        public DateOnly DepartureDate { get; set; }

        /// <summary>
        /// Время заезда.
        /// </summary>
        public TimeOnly ArrivalTime { get; set; }

        /// <summary>
        /// Время выезда.
        /// </summary>
        public TimeOnly DepartureTime { get; set; }

        /// <summary>
        /// ФИО гостя.
        /// </summary>
        public string GuestName { get; set; } = string.Empty;

        /// <summary>
        /// Контактный телефон гостя.
        /// </summary>
        public string GuestPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Количество гостей.
        /// </summary>
        public int GuestCount { get; set; }

        /// <summary>
        /// Количество оплачиваемых ночей.
        /// </summary>
        public int Nights { get; set; }

        /// <summary>
        /// Итоговая стоимость проживания.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Трёхбуквенный код валюты.
        /// </summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Текущий статус бронирования.
        /// </summary>
        public ReservationStatus Status { get; set; }

        /// <summary>
        /// Момент создания бронирования в UTC.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Момент отмены бронирования в UTC, если оно отменено.
        /// </summary>
        public DateTimeOffset? CancelledAt { get; set; }
    }
}
