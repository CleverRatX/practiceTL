namespace WebApi.Models.Properties
{
    /// <summary>
    /// Средство размещения.
    /// </summary>
    public class PropertyResponse
    {
        /// <summary>
        /// Идентификатор средства размещения.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название средства размещения.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Страна, в которой находится средство размещения.
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Город, в котором находится средство размещения.
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Почтовый адрес средства размещения.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Широта.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота.
        /// </summary>
        public double Longitude { get; set; }
    }
}
