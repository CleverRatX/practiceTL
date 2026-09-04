using Domain.Models;
using WebApi.Dto.Search;

namespace WebApi.Mapping
{
    public static class SearchMapping
    {
        public static SearchCriteria ToCriteria( this SearchRequest request )
        {
            return new SearchCriteria(
                request.City,
                request.ArrivalDate!.Value,
                request.DepartureDate!.Value,
                request.Guests,
                request.MaxPrice );
        }

        public static SearchOptionResponse ToResponse( this SearchResult option )
        {
            return new SearchOptionResponse(
                option.Property.Id,
                option.Property.Name,
                option.Property.Country,
                option.Property.City,
                option.Property.Address,
                option.RoomType.Id,
                option.RoomType.Name,
                option.RoomType.DailyPrice,
                option.RoomType.MinPersonCount,
                option.RoomType.MaxPersonCount,
                option.RoomType.Services,
                option.RoomType.Amenities,
                option.Nights,
                option.Total,
                option.RoomType.Currency );
        }

        public static IReadOnlyList<SearchOptionResponse> ToResponse( this IReadOnlyList<SearchResult> options )
        {
            return options
                .Select( option => option.ToResponse() )
                .ToList();
        }
    }
}
