using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware( RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync( HttpContext context )
        {
            try
            {
                await _next( context );
            }
            catch ( DomainException exception )
            {
                await WriteProblemAsync( context, GetStatusCode( exception ), exception.Message );
            }
            catch ( Exception exception )
            {
                _logger.LogError( exception, "Необработанная ошибка при обработке запроса {Path}.", context.Request.Path );

                await WriteProblemAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Внутренняя ошибка сервера." );
            }
        }

        private static int GetStatusCode( DomainException exception )
        {
            return exception switch
            {
                EntityNotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                DomainValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private static async Task WriteProblemAsync( HttpContext context, int statusCode, string message )
        {
            if ( context.Response.HasStarted )
            {
                return;
            }

            ProblemDetails problemDetails = new()
            {
                Status = statusCode,
                Title = message,
                Instance = context.Request.Path
            };

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync( problemDetails );
        }
    }
}
