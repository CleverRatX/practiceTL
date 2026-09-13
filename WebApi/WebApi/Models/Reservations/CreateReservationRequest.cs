using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Reservations
{
    /// <summary>
    /// Данные для создания бронирования.
    /// </summary>
    public class CreateReservationRequest
    {
        /// <summary>
        /// Идентификатор средства размещения.
        /// </summary>
        [Required]
        public Guid? PropertyId { get; set; }

        /// <summary>
        /// Идентификатор бронируемой категории номера.
        /// </summary>
        [Required]
        public Guid? RoomTypeId { get; set; }

        /// <summary>
        /// Дата заезда. Заезд возможен начиная с сегодняшнего дня.
        /// </summary>
        [Required]
        public DateOnly? ArrivalDate { get; set; }

        /// <summary>
        /// Дата выезда, должна быть позже даты заезда.
        /// </summary>
        [Required]
        public DateOnly? DepartureDate { get; set; }

        /// <summary>
        /// ФИО гостя.
        /// </summary>
        [Required]
        [MaxLength( 200 )]
        [DefaultValue( "Иванов Иван Иванович" )]
        public string GuestName { get; set; } = string.Empty;

        /// <summary>
        /// Контактный телефон гостя.
        /// </summary>
        [Required]
        [Phone]
        [MaxLength( 30 )]
        [DefaultValue( "+79123456789" )]
        public string GuestPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Количество гостей, должно попадать в вместимость категории номера.
        /// </summary>
        [Range( 1, 50 )]
        [DefaultValue( 1 )]
        public int GuestCount { get; set; } = 1;
    }
}
