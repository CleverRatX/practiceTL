using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Properties
{
    public class GetPropertiesService : IGetPropertiesService
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertiesService( IPropertyRepository propertyRepository )
        {
            _propertyRepository = propertyRepository;
        }

        public Task<IReadOnlyList<Property>> GetAllAsync()
        {
            return _propertyRepository.GetAllAsync();
        }
    }
}
