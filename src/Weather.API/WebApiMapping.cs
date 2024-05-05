
using Weather.API.Dtos;

namespace Weather.API;

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
