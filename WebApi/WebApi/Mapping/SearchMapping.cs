using Application.Dto;
using WebApi.Models.Search;

namespace WebApi.Mapping
{
    public static class SearchMapping
    {
        public static SearchCriteria ToCriteria( this SearchRequest request )
        {
            return new SearchCriteria
            {
                City = request.City,
                ArrivalDate = request.ArrivalDate!.Value,
                DepartureDate = request.DepartureDate!.Value,
                GuestCount = request.GuestCount,
                MaxDailyPrice = request.MaxDailyPrice
            };
        }

        public static SearchOptionResponse ToResponse( this SearchResult option )
        {
            return new SearchOptionResponse
            {
                PropertyId = option.Property.Id,
                PropertyName = option.Property.Name,
                Country = option.Property.Country,
                City = option.Property.City,
                Address = option.Property.Address,
                RoomTypeId = option.RoomType.Id,
                RoomTypeName = option.RoomType.Name,
                DailyPrice = option.RoomType.DailyPrice,
                MinPersonCount = option.RoomType.MinPersonCount,
                MaxPersonCount = option.RoomType.MaxPersonCount,
                Services = option.RoomType.Services,
                Amenities = option.RoomType.Amenities,
                Nights = option.Nights,
                Total = option.Total,
                Currency = option.RoomType.Currency
            };
        }

        public static IReadOnlyList<SearchOptionResponse> ToResponse( this IReadOnlyList<SearchResult> options )
        {
            return options
                .Select( ToResponse )
                .ToList();
        }
    }
}
