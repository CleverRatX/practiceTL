using Application.Dto;
using Domain.Entities;
using WebApi.Models.Properties;

namespace WebApi.Mapping
{
    public static class PropertyMapping
    {
        public static PropertyResponse ToResponse( this Property property )
        {
            return new PropertyResponse
            {
                Id = property.Id,
                Name = property.Name,
                Country = property.Country,
                City = property.City,
                Address = property.Address,
                Latitude = property.Latitude,
                Longitude = property.Longitude
            };
        }

        public static IReadOnlyList<PropertyResponse> ToResponse( this IReadOnlyList<Property> properties )
        {
            return properties
                .Select( ToResponse )
                .ToList();
        }

        public static PropertyData ToData( this CreatePropertyRequest request )
        {
            return new PropertyData
            {
                Name = request.Name,
                Country = request.Country,
                City = request.City,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }

        public static PropertyData ToData( this UpdatePropertyRequest request )
        {
            return new PropertyData
            {
                Name = request.Name,
                Country = request.Country,
                City = request.City,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }
    }
}
