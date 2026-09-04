using Domain.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryInfrastructure( this IServiceCollection services )
        {
            services.AddSingleton<InMemoryStorage>();

            services.AddScoped<IPropertyRepository, InMemoryPropertyRepository>();
            services.AddScoped<IRoomTypeRepository, InMemoryRoomTypeRepository>();
            services.AddScoped<IReservationRepository, InMemoryReservationRepository>();

            return services;
        }
    }
}
