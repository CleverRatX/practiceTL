using Domain.Exceptions;
using WebApi.Models.Errors;

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
                await WriteErrorAsync( context, GetStatusCode( exception ), GetErrorCode( exception ), exception.Message );
            }
            catch ( Exception exception )
            {
                _logger.LogError( exception, "Необработанная ошибка при обработке запроса {Path}.", context.Request.Path );

                await WriteErrorAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    ErrorCodes.InternalError,
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

        private static string GetErrorCode( DomainException exception )
        {
            return exception switch
            {
                EntityNotFoundException => ErrorCodes.EntityNotFound,
                ConflictException => ErrorCodes.Conflict,
                DomainValidationException => ErrorCodes.ValidationError,
                _ => ErrorCodes.InternalError
            };
        }

        private static async Task WriteErrorAsync( HttpContext context, int statusCode, string errorCode, string message )
        {
            if ( context.Response.HasStarted )
            {
                return;
            }

            ErrorResponse[] errors =
            [
                new ErrorResponse
                {
                    ErrorCode = errorCode,
                    Message = message
                }
            ];

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync( errors );
        }
    }
}
