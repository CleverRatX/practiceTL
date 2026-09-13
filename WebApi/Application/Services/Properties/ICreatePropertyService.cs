using Application.Dto;
using Domain.Entities;

namespace Application.Services.Properties
{
    public interface ICreatePropertyService
    {
        Task<Property> CreateAsync( PropertyData data );
    }
}