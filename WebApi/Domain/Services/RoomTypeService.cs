using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;

namespace Domain.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IReservationRepository _reservationRepository;

        public RoomTypeService(
            IRoomTypeRepository roomTypeRepository,
            IPropertyRepository propertyRepository,
            IReservationRepository reservationRepository )
        {
            _roomTypeRepository = roomTypeRepository;
            _propertyRepository = propertyRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<IReadOnlyList<RoomType>> GetByPropertyAsync( Guid propertyId, CancellationToken cancellationToken )
        {
            await EnsurePropertyExistsAsync( propertyId, cancellationToken );

            return await _roomTypeRepository.GetByPropertyIdAsync( propertyId, cancellationToken );
        }

        public async Task<RoomType> GetByIdAsync( Guid id, CancellationToken cancellationToken )
        {
            RoomType? roomType = await _roomTypeRepository.GetByIdAsync( id, cancellationToken );

            if ( roomType is null )
            {
                throw new EntityNotFoundException( $"Категория номера {id} не найдена." );
            }

            return roomType;
        }

        public async Task<RoomType> CreateAsync( Guid propertyId, RoomTypeData data, CancellationToken cancellationToken )
        {
            await EnsurePropertyExistsAsync( propertyId, cancellationToken );

            RoomType roomType = new( propertyId, data );

            await _roomTypeRepository.AddAsync( roomType, cancellationToken );

            return roomType;
        }

        public async Task<RoomType> UpdateAsync( Guid id, RoomTypeData data, CancellationToken cancellationToken )
        {
            RoomType roomType = await GetByIdAsync( id, cancellationToken );

            roomType.Update( data );

            await _roomTypeRepository.UpdateAsync( roomType, cancellationToken );

            return roomType;
        }

        public async Task DeleteAsync( Guid id, CancellationToken cancellationToken )
        {
            RoomType roomType = await GetByIdAsync( id, cancellationToken );

            bool hasReservations = await _reservationRepository.HasActiveByRoomTypeAsync( roomType.Id, cancellationToken );

            if ( hasReservations )
            {
                throw new ConflictException(
                    "Нельзя удалить категорию номера, по которой есть действующие бронирования." );
            }

            await _roomTypeRepository.DeleteAsync( roomType.Id, cancellationToken );
        }

        private async Task EnsurePropertyExistsAsync( Guid propertyId, CancellationToken cancellationToken )
        {
            Property? property = await _propertyRepository.GetByIdAsync( propertyId, cancellationToken );

            if ( property is null )
            {
                throw new EntityNotFoundException( $"Средство размещения {propertyId} не найдено." );
            }
        }
    }
}
