using Application;
using Infrastructure.Foundation;
using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Binding;
using WebApi.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

string connectionString = builder.Configuration.GetConnectionString( "Booking" )
    ?? throw new InvalidOperationException( "В конфигурации не задана строка подключения \"Booking\"." );

builder.Services
    .AddApplication()
    .AddFoundation( connectionString );

builder.Services.AddApiControllers();
builder.Services.AddApiDocumentation();

WebApplication app = builder.Build();

using ( IServiceScope scope = app.Services.CreateScope() )
{
    BookingDbContext context = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    context.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI( options =>
{
    options.SwaggerEndpoint( $"/swagger/{ApiDocuments.Properties}/swagger.json", "PropertiesApi" );
    options.SwaggerEndpoint( $"/swagger/{ApiDocuments.Reservations}/swagger.json", "ReservationApi" );
} );

app.MapControllers();

app.Run();
