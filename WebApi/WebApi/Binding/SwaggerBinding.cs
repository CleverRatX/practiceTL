using System.Reflection;
using Microsoft.OpenApi;

namespace WebApi.Binding
{
    public static class SwaggerBinding
    {
        public static IServiceCollection AddApiDocumentation( this IServiceCollection services )
        {
            services.AddSwaggerGen( options =>
            {
                options.SwaggerDoc( ApiDocuments.Properties, new()
                {
                    Title = "PropertiesApi",
                    Version = "v1",
                    Description = "Управление средствами размещения и категориями номеров."
                } );

                options.SwaggerDoc( ApiDocuments.Reservations, new()
                {
                    Title = "ReservationApi",
                    Version = "v1",
                    Description = "Поиск вариантов размещения, создание, просмотр и отмена бронирований."
                } );

                options.MapType<TimeOnly>( () => new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Example = "14:00:00"
                } );

                IncludeXmlComments( options );
            } );

            return services;
        }

        private static void IncludeXmlComments( Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options )
        {
            string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlFilePath = Path.Combine( AppContext.BaseDirectory, xmlFileName );

            if ( File.Exists( xmlFilePath ) )
            {
                options.IncludeXmlComments( xmlFilePath );
            }
        }
    }
}
