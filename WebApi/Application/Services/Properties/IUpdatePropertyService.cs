using Application.Dto;
using Domain.Entities;

namespace Application.Services.Properties
{
    public interface IUpdatePropertyService
    {
        Task<Property> UpdateAsync( Guid id, PropertyData data );
    }
}
