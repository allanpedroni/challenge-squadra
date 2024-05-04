
namespace Weather.API;

public static class WebApiMapping
{
    public static TypeAdapterConfig Default()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<WeatherForecastAuditEntity, WeatherForecastAudit>();

        return config;
    }
}
