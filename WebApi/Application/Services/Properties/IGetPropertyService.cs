using Domain.Entities;

namespace Application.Services.Properties
{
    public interface IGetPropertyService
    {
        Task<Property> GetByIdAsync( Guid id );
    }
}
