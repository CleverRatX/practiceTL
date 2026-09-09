using Application.Dto;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Properties
{
    public class CreatePropertyService : ICreatePropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePropertyService( IPropertyRepository propertyRepository, IUnitOfWork unitOfWork )
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Property> CreateAsync( PropertyData data )
        {
            Property property = new(
                data.Name,
                data.Country,
                data.City,
                data.Address,
                data.Latitude,
                data.Longitude );

            _propertyRepository.Add( property );

            await _unitOfWork.SaveChangesAsync();

            return property;
        }
    }
}
