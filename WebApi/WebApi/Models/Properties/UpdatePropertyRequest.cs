using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Properties
{
    /// <summary>
    /// Данные для обновления средства размещения.
    /// </summary>
    public class UpdatePropertyRequest
    {
        /// <summary>
        /// Название средства размещения.
        /// </summary>
        [Required]
        [MaxLength( 200 )]
        [DefaultValue( "Отель" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Страна, в которой находится средство размещения.
        /// </summary>
        [Required]
        [MaxLength( 100 )]
        [DefaultValue( "Россия" )]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Город, в котором находится средство размещения.
        /// </summary>
        [Required]
        [MaxLength( 100 )]
        [DefaultValue( "Йошкар-Ола" )]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Почтовый адрес средства размещения.
        /// </summary>
        [Required]
        [MaxLength( 300 )]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Широта, от -90 до 90.
        /// </summary>
        [Range( -90, 90 )]
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота, от -180 до 180.
        /// </summary>
        [Range( -180, 180 )]
        public double Longitude { get; set; }
    }
}
