using Domain.Validation;

namespace Domain.Entities
{
    public class Property
    {
        private const double LatitudeLimit = 90;
        private const double LongitudeLimit = 180;

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Country { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;

        public string Address { get; private set; } = string.Empty;

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        private Property()
        {
        }

        public Property(
            string name,
            string country,
            string city,
            string address,
            double latitude,
            double longitude )
            : this( Guid.NewGuid(), name, country, city, address, latitude, longitude )
        {
        }

        public Property(
            Guid id,
            string name,
            string country,
            string city,
            string address,
            double latitude,
            double longitude )
        {
            Id = id;

            Update( name, country, city, address, latitude, longitude );
        }

        public void Update(
            string name,
            string country,
            string city,
            string address,
            double latitude,
            double longitude )
        {
            Name = Validator.NormalizedText( name, "Название средства размещения не может быть пустым." );
            Country = Validator.NormalizedText( country, "Страна не может быть пустой." );
            City = Validator.NormalizedText( city, "Город не может быть пустым." );
            Address = Validator.NormalizedText( address, "Адрес не может быть пустым." );
            Latitude = Validator.Coordinate(
                latitude,
                LatitudeLimit,
                $"Широта должна быть в диапазоне от -{LatitudeLimit} до {LatitudeLimit}." );
            Longitude = Validator.Coordinate(
                longitude,
                LongitudeLimit,
                $"Долгота должна быть в диапазоне от -{LongitudeLimit} до {LongitudeLimit}." );
        }
    }
}
