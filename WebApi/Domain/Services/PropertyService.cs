using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;

namespace Domain.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IReservationRepository _reservationRepository;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IRoomTypeRepository roomTypeRepository,
            IReservationRepository reservationRepository )
        {
            _propertyRepository = propertyRepository;
            _roomTypeRepository = roomTypeRepository;
            _reservationRepository = reservationRepository;
        }

        public Task<IReadOnlyList<Property>> GetAllAsync( CancellationToken cancellationToken )
        {
            return _propertyRepository.GetAllAsync( cancellationToken );
        }

        public async Task<Property> GetByIdAsync( Guid id, CancellationToken cancellationToken )
        {
            Property? property = await _propertyRepository.GetByIdAsync( id, cancellationToken );

            if ( property is null )
            {
                throw new EntityNotFoundException( $"Средство размещения {id} не найдено." );
            }

            return property;
        }

        public async Task<Property> CreateAsync( PropertyData data, CancellationToken cancellationToken )
        {
            Property property = new( data );

            await _propertyRepository.AddAsync( property, cancellationToken );

            return property;
        }

        public async Task<Property> UpdateAsync( Guid id, PropertyData data, CancellationToken cancellationToken )
        {
            Property property = await GetByIdAsync( id, cancellationToken );

            property.Update( data );

            await _propertyRepository.UpdateAsync( property, cancellationToken );

            return property;
        }

        public async Task DeleteAsync( Guid id, CancellationToken cancellationToken )
        {
            Property property = await GetByIdAsync( id, cancellationToken );

            bool hasReservations = await _reservationRepository.HasActiveByPropertyAsync( property.Id, cancellationToken );

            if ( hasReservations )
            {
                throw new ConflictException(
                    "Нельзя удалить средство размещения, по которому есть действующие бронирования." );
            }

            await _roomTypeRepository.DeleteByPropertyIdAsync( property.Id, cancellationToken );
            await _propertyRepository.DeleteAsync( property.Id, cancellationToken );
        }
    }
}
