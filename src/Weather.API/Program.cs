using Weather.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .AddBaseServices()
    .AddEntityFramework()
    .AddSwagger();

builder.Services
    .AddHttpClient()
    .AddInfraestructureServices(builder.Configuration)
    .AddServices();

var app = builder.Build();

app.AddSwagger();

app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

await app.AddMigrationsAsync(builder);

app.Run();
