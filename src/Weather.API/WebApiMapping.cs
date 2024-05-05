
using System.Diagnostics.CodeAnalysis;
using Weather.API.Dtos;

namespace Weather.API;

[ExcludeFromCodeCoverage]
public static class WebApiMapping
{
    public static TypeAdapterConfig Default()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<WeatherForecastAuditEntity, WeatherForecastAuditGetResponse>();

        config.NewConfig<WeatherForecast, WeatherForecastGetResponse>();

        return config;
    }
}
