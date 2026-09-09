namespace WebApi.Models.Search
{
    /// <summary>
    /// Свободный вариант размещения, найденный по условиям поиска.
    /// </summary>
    public class SearchOptionResponse
    {
        /// <summary>
        /// Идентификатор средства размещения.
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Название средства размещения.
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

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
        /// Идентификатор категории номера.
        /// </summary>
        public Guid RoomTypeId { get; set; }

        /// <summary>
        /// Название категории номера.
        /// </summary>
        public string RoomTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Стоимость проживания за одну ночь.
        /// </summary>
        public decimal DailyPrice { get; set; }

        /// <summary>
        /// Минимальное число гостей в номере.
        /// </summary>
        public int MinPersonCount { get; set; }

        /// <summary>
        /// Максимальное число гостей в номере.
        /// </summary>
        public int MaxPersonCount { get; set; }

        /// <summary>
        /// Услуги, входящие в стоимость проживания.
        /// </summary>
        public IReadOnlyList<string> Services { get; set; } = [];

        /// <summary>
        /// Удобства номера.
        /// </summary>
        public IReadOnlyList<string> Amenities { get; set; } = [];

        /// <summary>
        /// Количество оплачиваемых ночей.
        /// </summary>
        public int Nights { get; set; }

        /// <summary>
        /// Итоговая стоимость проживания за весь период.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Трёхбуквенный код валюты.
        /// </summary>
        public string Currency { get; set; } = string.Empty;
    }
}
