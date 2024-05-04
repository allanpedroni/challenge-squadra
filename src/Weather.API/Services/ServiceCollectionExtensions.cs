using Weather.API.Models.Services;

namespace Weather.API.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IWeatherForecastService, WeatherForecastService>();

        return services;
    }
}
