using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services.Properties
{
    public class GetPropertyService : IGetPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertyService( IPropertyRepository propertyRepository )
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<Property> GetByIdAsync( Guid id )
        {
            Property? property = await _propertyRepository.GetByIdAsync( id );

            if ( property is null )
            {
                throw new EntityNotFoundException( $"Средство размещения с id: {id} не найдено." );
            }

            return property;
        }
    }
}
