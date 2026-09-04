using Domain.Exceptions;
using Domain.Models;
using Domain.Validation;

namespace Domain.Entities
{
    public class RoomType
    {
        public RoomType( Guid propertyId, RoomTypeData data )
            : this( Guid.NewGuid(), propertyId, data )
        {
        }

        public RoomType( Guid id, Guid propertyId, RoomTypeData data )
        {
            Id = id;
            PropertyId = propertyId;
            SetDetails( data );
        }

        public Guid Id { get; }

        public Guid PropertyId { get; }

        public string Name { get; private set; } = string.Empty;

        public decimal DailyPrice { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        public int MinPersonCount { get; private set; }

        public int MaxPersonCount { get; private set; }

        public int RoomCount { get; private set; }

        public IReadOnlyList<string> Services { get; private set; } = [];

        public IReadOnlyList<string> Amenities { get; private set; } = [];

        public void Update( RoomTypeData data )
        {
            SetDetails( data );
        }

        public bool FitsGuests( int guestCount )
        {
            return guestCount >= MinPersonCount && guestCount <= MaxPersonCount;
        }

        public decimal CalculateTotal( int nights )
        {
            if ( nights <= 0 )
            {
                throw new DomainValidationException( "Количество ночей должно быть больше нуля." );
            }

            return DailyPrice * nights;
        }

        private void SetDetails( RoomTypeData data )
        {
            Name = Validated.Text( data.Name, "Название категории номера не может быть пустым." );
            DailyPrice = Validated.PositiveAmount( data.DailyPrice, "Цена за ночь должна быть больше нуля." );
            Currency = Validated.Currency( data.Currency );
            MinPersonCount = Validated.PositiveCount( data.MinPersonCount, "Минимальное число гостей должно быть больше нуля." );
            MaxPersonCount = Validated.PositiveCount( data.MaxPersonCount, "Максимальное число гостей должно быть больше нуля." );
            RoomCount = Validated.PositiveCount( data.RoomCount, "Количество номеров категории должно быть больше нуля." );

            if ( MinPersonCount > MaxPersonCount )
            {
                throw new DomainValidationException( "Минимальное число гостей не может быть больше максимального." );
            }

            Services = data.Services?.ToList() ?? [];
            Amenities = data.Amenities?.ToList() ?? [];
        }
    }
}
