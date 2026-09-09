using Application.Dto;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Properties
{
    public class UpdatePropertyService : IUpdatePropertyService
    {
        private readonly IGetPropertyService _getPropertyService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePropertyService( IGetPropertyService getPropertyService, IUnitOfWork unitOfWork )
        {
            _getPropertyService = getPropertyService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Property> UpdateAsync( Guid id, PropertyData data )
        {
            Property property = await _getPropertyService.GetByIdAsync( id );

            property.Update(
                data.Name,
                data.Country,
                data.City,
                data.Address,
                data.Latitude,
                data.Longitude );

            await _unitOfWork.SaveChangesAsync();

            return property;
        }
    }
}
