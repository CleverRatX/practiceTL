using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services.RoomTypes
{
    public class GetPropertyRoomTypesService : IGetPropertyRoomTypesService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertyRoomTypesService(
            IRoomTypeRepository roomTypeRepository,
            IPropertyRepository propertyRepository )
        {
            _roomTypeRepository = roomTypeRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<IReadOnlyList<RoomType>> GetByPropertyIdAsync( Guid propertyId )
        {
            bool propertyExists = await _propertyRepository.ExistsAsync( propertyId );

            if ( !propertyExists )
            {
                throw new EntityNotFoundException( $"Средство размещения с id: {propertyId} не найдено." );
            }

            return await _roomTypeRepository.GetByPropertyIdAsync( propertyId );
        }
    }
}
