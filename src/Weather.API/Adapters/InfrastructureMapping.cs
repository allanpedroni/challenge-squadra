
namespace Weather.API;

public static class InfrastructureMapping
{
    public static TypeAdapterConfig Default()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<List, WeatherForecast>()
            .Map(dest => dest.Date, src => src.dt_txt)
            .Map(dest => dest.TemperatureMin, src => src.main.temp_min < 0 ? 0 : (int)src.main.temp_min)
            .Map(dest => dest.TemperatureMax, src => src.main.temp_max < 0 ? 0 : (int)src.main.temp_max)
            .Map(dest => dest.Summary, src => GetDescription(src))
            .Map(dest => dest.Icon, src => GetIcon(src));

        return config;
    }

    private static string GetDescription(Weather.API.Adapters.OpenWeatherAPI.Dtos.List list)
    {
        return list.weather.FirstOrDefault()?.description ?? string.Empty;
    }

    private static string GetIcon(Weather.API.Adapters.OpenWeatherAPI.Dtos.List list)
    {
        return list.weather.FirstOrDefault()?.icon ?? string.Empty;
    }
}
