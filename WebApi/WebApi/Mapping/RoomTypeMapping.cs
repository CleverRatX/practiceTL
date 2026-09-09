using Application.Dto;
using Domain.Entities;
using WebApi.Models.RoomTypes;

namespace WebApi.Mapping
{
    public static class RoomTypeMapping
    {
        public static RoomTypeResponse ToResponse( this RoomType roomType )
        {
            return new RoomTypeResponse
            {
                Id = roomType.Id,
                PropertyId = roomType.PropertyId,
                Name = roomType.Name,
                DailyPrice = roomType.DailyPrice,
                Currency = roomType.Currency,
                MinPersonCount = roomType.MinPersonCount,
                MaxPersonCount = roomType.MaxPersonCount,
                RoomCount = roomType.RoomCount,
                Services = roomType.Services,
                Amenities = roomType.Amenities
            };
        }

        public static IReadOnlyList<RoomTypeResponse> ToResponse( this IReadOnlyList<RoomType> roomTypes )
        {
            return roomTypes
                .Select( ToResponse )
                .ToList();
        }

        public static RoomTypeData ToData( this CreateRoomTypeRequest request )
        {
            return new RoomTypeData
            {
                Name = request.Name,
                DailyPrice = request.DailyPrice,
                Currency = request.Currency,
                MinPersonCount = request.MinPersonCount,
                MaxPersonCount = request.MaxPersonCount,
                RoomCount = request.RoomCount,
                Services = request.Services ?? [],
                Amenities = request.Amenities ?? []
            };
        }

        public static RoomTypeData ToData( this UpdateRoomTypeRequest request )
        {
            return new RoomTypeData
            {
                Name = request.Name,
                DailyPrice = request.DailyPrice,
                Currency = request.Currency,
                MinPersonCount = request.MinPersonCount,
                MaxPersonCount = request.MaxPersonCount,
                RoomCount = request.RoomCount,
                Services = request.Services ?? [],
                Amenities = request.Amenities ?? []
            };
        }
    }
}
