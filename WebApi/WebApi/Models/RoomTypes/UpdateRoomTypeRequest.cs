using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.RoomTypes
{
    /// <summary>
    /// Данные для обновления категории номера.
    /// </summary>
    public class UpdateRoomTypeRequest
    {
        /// <summary>
        /// Название категории номера.
        /// </summary>
        [Required]
        [MaxLength( 200 )]
        [DefaultValue( "Стандарт двухместный" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Стоимость проживания за одну ночь.
        /// </summary>
        [Range( 0.01, 1_000_000 )]
        [DefaultValue( 4500 )]
        public decimal DailyPrice { get; set; }

        /// <summary>
        /// Трёхбуквенный код валюты, например "RUB".
        /// </summary>
        [Required]
        [StringLength( 3, MinimumLength = 3 )]
        [DefaultValue( "RUB" )]
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Минимальное число гостей в номере.
        /// </summary>
        [Range( 1, 50 )]
        [DefaultValue( 1 )]
        public int MinPersonCount { get; set; } = 1;

        /// <summary>
        /// Максимальное число гостей в номере.
        /// </summary>
        [Range( 1, 50 )]
        [DefaultValue( 2 )]
        public int MaxPersonCount { get; set; } = 1;

        /// <summary>
        /// Количество номеров этой категории в средстве размещения.
        /// </summary>
        [Range( 1, 10_000 )]
        [DefaultValue( 5 )]
        public int RoomCount { get; set; } = 1;

        /// <summary>
        /// Услуги, входящие в стоимость проживания.
        /// </summary>
        public IReadOnlyList<string>? Services { get; set; }

        /// <summary>
        /// Удобства номера.
        /// </summary>
        public IReadOnlyList<string>? Amenities { get; set; }
    }
}
