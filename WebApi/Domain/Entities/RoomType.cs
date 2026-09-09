using Domain.Exceptions;
using Domain.Validation;

namespace Domain.Entities
{
    public class RoomType
    {
        public Guid Id { get; private set; }

        public Guid PropertyId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public decimal DailyPrice { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        public int MinPersonCount { get; private set; }

        public int MaxPersonCount { get; private set; }

        public int RoomCount { get; private set; }

        public IReadOnlyList<string> Services { get; private set; } = [];

        public IReadOnlyList<string> Amenities { get; private set; } = [];

        private RoomType()
        {
        }

        public RoomType(
            Guid propertyId,
            string name,
            decimal dailyPrice,
            string currency,
            int minPersonCount,
            int maxPersonCount,
            int roomCount,
            IReadOnlyList<string>? services,
            IReadOnlyList<string>? amenities )
            : this(
                Guid.NewGuid(),
                propertyId,
                name,
                dailyPrice,
                currency,
                minPersonCount,
                maxPersonCount,
                roomCount,
                services,
                amenities )
        {
        }

        public RoomType(
            Guid id,
            Guid propertyId,
            string name,
            decimal dailyPrice,
            string currency,
            int minPersonCount,
            int maxPersonCount,
            int roomCount,
            IReadOnlyList<string>? services,
            IReadOnlyList<string>? amenities )
        {
            Id = id;
            PropertyId = propertyId;

            Update( name, dailyPrice, currency, minPersonCount, maxPersonCount, roomCount, services, amenities );
        }

        public void Update(
            string name,
            decimal dailyPrice,
            string currency,
            int minPersonCount,
            int maxPersonCount,
            int roomCount,
            IReadOnlyList<string>? services,
            IReadOnlyList<string>? amenities )
        {
            Name = Validator.NormalizedText( name, "Название категории номера не может быть пустым." );
            DailyPrice = Validator.Positive( dailyPrice, "Цена за ночь должна быть больше нуля." );
            Currency = Validator.NormalizedCurrencyCode( currency );

            MinPersonCount = Validator.PositiveCount( minPersonCount, "Минимальное число гостей должно быть больше нуля." );
            MaxPersonCount = Validator.PositiveCount( maxPersonCount, "Максимальное число гостей должно быть больше нуля." );

            if ( MinPersonCount > MaxPersonCount )
            {
                throw new DomainValidationException( "Минимальное число гостей не может быть больше максимального." );
            }

            RoomCount = Validator.PositiveCount( roomCount, "Количество номеров категории должно быть больше нуля." );

            Services = services?.ToList() ?? [];
            Amenities = amenities?.ToList() ?? [];
        }
    }
}
