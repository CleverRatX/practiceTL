using System.Text.Json.Serialization;
using Domain.Services;
using Infrastructure;
using WebApi;
using WebApi.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddSingleton( TimeProvider.System );
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddInMemoryInfrastructure();

builder.Services
    .AddControllers()
    .AddJsonOptions( options =>
    {
        options.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
    } );

builder.Services.AddSwaggerGen( options =>
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
} );

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI( options =>
{
    options.SwaggerEndpoint( $"/swagger/{ApiDocuments.Properties}/swagger.json", "PropertiesApi" );
    options.SwaggerEndpoint( $"/swagger/{ApiDocuments.Reservations}/swagger.json", "ReservationApi" );
} );

app.MapControllers();

app.Run();
