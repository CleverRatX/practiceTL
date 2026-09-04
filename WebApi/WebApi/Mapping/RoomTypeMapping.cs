using Domain.Entities;
using Domain.Models;
using WebApi.Dto.RoomTypes;

namespace WebApi.Mapping
{
    public static class RoomTypeMapping
    {
        public static RoomTypeResponse ToResponse( this RoomType roomType )
        {
            return new RoomTypeResponse(
                roomType.Id,
                roomType.PropertyId,
                roomType.Name,
                roomType.DailyPrice,
                roomType.Currency,
                roomType.MinPersonCount,
                roomType.MaxPersonCount,
                roomType.RoomCount,
                roomType.Services,
                roomType.Amenities );
        }

        public static IReadOnlyList<RoomTypeResponse> ToResponse( this IReadOnlyList<RoomType> roomTypes )
        {
            return roomTypes
                .Select( roomType => roomType.ToResponse() )
                .ToList();
        }

        public static RoomTypeData ToData( this CreateRoomTypeRequest request )
        {
            return new RoomTypeData(
                request.Name,
                request.DailyPrice,
                request.Currency,
                request.MinPersonCount,
                request.MaxPersonCount,
                request.RoomCount,
                request.Services ?? [],
                request.Amenities ?? [] );
        }

        public static RoomTypeData ToData( this UpdateRoomTypeRequest request )
        {
            return new RoomTypeData(
                request.Name,
                request.DailyPrice,
                request.Currency,
                request.MinPersonCount,
                request.MaxPersonCount,
                request.RoomCount,
                request.Services ?? [],
                request.Amenities ?? [] );
        }
    }
}
