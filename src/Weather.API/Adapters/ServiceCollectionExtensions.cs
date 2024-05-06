using System.Diagnostics.CodeAnalysis;
using Weather.API.Adapters.SqlServer;

namespace Weather.API.Adapters;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var openWeatherConfiguration = configuration.SafeGet<OpenWeatherConfiguration>();

        services.AddSingleton(openWeatherConfiguration ?? throw new ArgumentNullException(nameof(OpenWeatherConfiguration)));

        //OpenWeatherAPI
        services.AddScoped<IOpenWeatherAPIAdapter, OpenWeatherApiAdapter>();
        
        //Sqlserver - Entityframework
        services.AddScoped<IWeatherForecastAuditWriteSqlAdapter, WeatherForecastAuditSqlAdapter>();
        services.AddScoped<IWeatherForecastAuditReadSqlAdapter, WeatherForecastAuditSqlAdapter>();

        return services;
    }
}
