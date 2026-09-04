using Domain.Entities;
using Domain.Models;
using WebApi.Dto.Properties;

namespace WebApi.Mapping
{
    public static class PropertyMapping
    {
        public static PropertyResponse ToResponse( this Property property )
        {
            return new PropertyResponse(
                property.Id,
                property.Name,
                property.Country,
                property.City,
                property.Address,
                property.Latitude,
                property.Longitude );
        }

        public static IReadOnlyList<PropertyResponse> ToResponse( this IReadOnlyList<Property> properties )
        {
            return properties
                .Select( property => property.ToResponse() )
                .ToList();
        }

        public static PropertyData ToData( this CreatePropertyRequest request )
        {
            return new PropertyData(
                request.Name,
                request.Country,
                request.City,
                request.Address,
                request.Latitude,
                request.Longitude );
        }

        public static PropertyData ToData( this UpdatePropertyRequest request )
        {
            return new PropertyData(
                request.Name,
                request.Country,
                request.City,
                request.Address,
                request.Latitude,
                request.Longitude );
        }
    }
}
