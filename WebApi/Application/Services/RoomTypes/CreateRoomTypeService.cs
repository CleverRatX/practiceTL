using Application.Dto;
using Application.Persistence;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services.RoomTypes
{
    public class CreateRoomTypeService : ICreateRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoomTypeService(
            IRoomTypeRepository roomTypeRepository,
            IPropertyRepository propertyRepository,
            IUnitOfWork unitOfWork )
        {
            _roomTypeRepository = roomTypeRepository;
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoomType> CreateAsync( Guid propertyId, RoomTypeData data )
        {
            bool propertyExists = await _propertyRepository.ExistsAsync( propertyId );

            if ( !propertyExists )
            {
                throw new EntityNotFoundException( $"Средство размещения с id: {propertyId} не найдено." );
            }

            RoomType roomType = new(
                propertyId,
                data.Name,
                data.DailyPrice,
                data.Currency,
                data.MinPersonCount,
                data.MaxPersonCount,
                data.RoomCount,
                data.Services,
                data.Amenities );

            _roomTypeRepository.Add( roomType );
            await _unitOfWork.SaveChangesAsync();

            return roomType;
        }
    }
}
