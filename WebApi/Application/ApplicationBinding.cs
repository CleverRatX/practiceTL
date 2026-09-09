using Application.Services.Availability;
using Application.Services.Properties;
using Application.Services.Reservations;
using Application.Services.RoomTypes;
using Application.Services.Search;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application
{
    public static class ApplicationBinding
    {
        public static IServiceCollection AddApplication( this IServiceCollection services )
        {
            services.AddSingleton( TimeProvider.System );

            return services
                .AddPropertyManagement()
                .AddRoomTypeManagement()
                .AddReservationManagement()
                .AddSearchServices();
        }

        public static IServiceCollection AddPropertyManagement( this IServiceCollection services )
        {
            services.AddScoped<IGetPropertiesService, GetPropertiesService>();
            services.AddScoped<IGetPropertyService, GetPropertyService>();
            services.AddScoped<ICreatePropertyService, CreatePropertyService>();
            services.AddScoped<IUpdatePropertyService, UpdatePropertyService>();
            services.AddScoped<IDeletePropertyService, DeletePropertyService>();

            return services;
        }

        public static IServiceCollection AddRoomTypeManagement( this IServiceCollection services )
        {
            services.AddScoped<IGetPropertyRoomTypesService, GetPropertyRoomTypesService>();
            services.AddScoped<IGetRoomTypeService, GetRoomTypeService>();
            services.AddScoped<ICreateRoomTypeService, CreateRoomTypeService>();
            services.AddScoped<IUpdateRoomTypeService, UpdateRoomTypeService>();
            services.AddScoped<IDeleteRoomTypeService, DeleteRoomTypeService>();

            return services;
        }

        public static IServiceCollection AddReservationManagement( this IServiceCollection services )
        {
            services.TryAddScoped<IRoomTypeAvailabilityService, RoomTypeAvailabilityService>();
            services.AddScoped<IGetReservationsService, GetReservationsService>();
            services.AddScoped<IGetReservationService, GetReservationService>();
            services.AddScoped<ICreateReservationService, CreateReservationService>();
            services.AddScoped<ICancelReservationService, CancelReservationService>();

            return services;
        }

        public static IServiceCollection AddSearchServices( this IServiceCollection services )
        {
            services.TryAddScoped<IRoomTypeAvailabilityService, RoomTypeAvailabilityService>();
            services.AddScoped<IAccommodationSearchService, AccommodationSearchService>();

            return services;
        }
    }
}
