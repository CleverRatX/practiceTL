using Domain.Repositories;
using Infrastructure.Foundation.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Foundation
{
    public static class FoundationBinding
    {
        public static IServiceCollection AddFoundation( this IServiceCollection services, string connectionString )
        {
            services.AddDbContext<BookingDbContext>( options => options.UseSqlServer( connectionString ) );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            return services;
        }
    }
}
