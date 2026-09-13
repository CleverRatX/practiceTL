namespace WebApi.Models.RoomTypes
{
    /// <summary>
    /// Категория номера средства размещения.
    /// </summary>
    public class RoomTypeResponse
    {
        /// <summary>
        /// Идентификатор категории номера.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор средства размещения, которому принадлежит категория.
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Название категории номера.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Стоимость проживания за одну ночь.
        /// </summary>
        public decimal DailyPrice { get; set; }

        /// <summary>
        /// Трёхбуквенный код валюты.
        /// </summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Минимальное число гостей в номере.
        /// </summary>
        public int MinPersonCount { get; set; }

        /// <summary>
        /// Максимальное число гостей в номере.
        /// </summary>
        public int MaxPersonCount { get; set; }

        /// <summary>
        /// Количество номеров этой категории.
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// Услуги, входящие в стоимость проживания.
        /// </summary>
        public IReadOnlyList<string> Services { get; set; } = [];

        /// <summary>
        /// Удобства номера.
        /// </summary>
        public IReadOnlyList<string> Amenities { get; set; } = [];
    }
}
