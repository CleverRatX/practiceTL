namespace WebApi.Models.Reservations
{
    /// <summary>
    /// Условия отбора бронирований.
    /// </summary>
    public class ReservationFilterRequest
    {
        /// <summary>
        /// Отбор по средству размещения.
        /// </summary>
        public Guid? PropertyId { get; set; }

        /// <summary>
        /// Отбор по категории номера.
        /// </summary>
        public Guid? RoomTypeId { get; set; }

        /// <summary>
        /// Отбор по части ФИО гостя.
        /// </summary>
        public string? GuestName { get; set; }

        /// <summary>
        /// Нижняя граница даты заезда.
        /// </summary>
        public DateOnly? ArrivalDateFrom { get; set; }

        /// <summary>
        /// Верхняя граница даты выезда.
        /// </summary>
        public DateOnly? DepartureDateTo { get; set; }

        /// <summary>
        /// Показывать ли отменённые бронирования.
        /// </summary>
        public bool IncludeCancelled { get; set; }
    }
}
