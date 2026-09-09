using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Search
{
    /// <summary>
    /// Условия поиска вариантов размещения.
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Город, в котором ищем размещение.
        /// </summary>
        [Required]
        [DefaultValue( "Йошкар-Ола" )]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Планируемая дата заезда.
        /// </summary>
        [Required]
        public DateOnly? ArrivalDate { get; set; }

        /// <summary>
        /// Планируемая дата выезда, должна быть позже даты заезда.
        /// </summary>
        [Required]
        public DateOnly? DepartureDate { get; set; }

        /// <summary>
        /// Количество гостей.
        /// </summary>
        [Range( 1, 50 )]
        [DefaultValue( 1 )]
        public int GuestCount { get; set; } = 1;

        /// <summary>
        /// Максимальная цена за ночь, если не задана, ограничения по цене нет.
        /// </summary>
        public decimal? MaxDailyPrice { get; set; }
    }
}
