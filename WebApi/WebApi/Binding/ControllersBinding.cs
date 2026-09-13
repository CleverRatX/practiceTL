using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Models.Errors;

namespace WebApi.Binding
{
    public static class ControllersBinding
    {
        public static IServiceCollection AddApiControllers( this IServiceCollection services )
        {
            services
                .AddControllers()
                .AddJsonOptions( options =>
                {
                    options.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
                } );

            services.Configure<ApiBehaviorOptions>( options =>
            {
                options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult( CollectErrors( context.ModelState ) );
            } );

            return services;
        }

        private static List<ErrorResponse> CollectErrors( ModelStateDictionary modelState )
        {
            List<ErrorResponse> errors = new();

            foreach ( string key in modelState.Keys )
            {
                ModelStateEntry? entry = modelState[ key ];

                if ( entry is null )
                {
                    continue;
                }

                foreach ( ModelError error in entry.Errors )
                {
                    string message = string.IsNullOrWhiteSpace( error.ErrorMessage )
                        ? "Некорректное значение."
                        : error.ErrorMessage;

                    errors.Add( new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.ValidationError,
                        Message = string.IsNullOrWhiteSpace( key ) ? message : $"{key}: {message}"
                    } );
                }
            }

            return errors;
        }
    }
}
