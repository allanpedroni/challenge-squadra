namespace Weather.API.Models.Services;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetFor5DaysByCityNameAsync(string cityName);
    Task<IEnumerable<WeatherForecastAuditEntity>> GetAuditByCityNameAsync(string cityName);
}
