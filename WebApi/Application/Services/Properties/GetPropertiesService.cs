using Domain.Repositories;
using Domain.Entities;

namespace Application.Services.Properties
{
    public class GetPropertiesService : IGetPropertiesService
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertiesService( IPropertyRepository propertyRepository )
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IReadOnlyList<Property>> GetAllAsync()
        {
            return await _propertyRepository.GetAllAsync();
        }
    }
}
