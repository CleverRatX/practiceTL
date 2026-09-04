using Domain.Models;
using Domain.Validation;

namespace Domain.Entities
{
    public class Property
    {
        private const double LatitudeLimit = 90;
        private const double LongitudeLimit = 180;

        public Property( PropertyData data )
            : this( Guid.NewGuid(), data )
        {
        }

        public Property( Guid id, PropertyData data )
        {
            Id = id;
            SetDetails( data );
        }

        public Guid Id { get; }

        public string Name { get; private set; } = string.Empty;

        public string Country { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;

        public string Address { get; private set; } = string.Empty;

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public void Update( PropertyData data )
        {
            SetDetails( data );
        }

        private void SetDetails( PropertyData data )
        {
            Name = Validated.Text( data.Name, "Название средства размещения не может быть пустым." );
            Country = Validated.Text( data.Country, "Страна не может быть пустой." );
            City = Validated.Text( data.City, "Город не может быть пустым." );
            Address = Validated.Text( data.Address, "Адрес не может быть пустым." );
            Latitude = Validated.Coordinate( data.Latitude, LatitudeLimit, "Широта должна быть в диапазоне от -90 до 90." );
            Longitude = Validated.Coordinate( data.Longitude, LongitudeLimit, "Долгота должна быть в диапазоне от -180 до 180." );
        }
    }
}
