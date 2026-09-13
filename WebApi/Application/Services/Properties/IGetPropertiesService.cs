using Domain.Entities;

namespace Application.Services.Properties
{
    public interface IGetPropertiesService
    {
        Task<IReadOnlyList<Property>> GetAllAsync();
    }
}
