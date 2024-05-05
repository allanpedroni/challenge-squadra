using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Weather.API.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .MinimumLevel.Override("Weather.API", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Async(a => a.Console(new JsonFormatter()))
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;
