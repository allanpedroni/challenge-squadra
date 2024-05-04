namespace Weather.API.Models.Adapters;

public interface IOpenWeatherAPIAdapter
{
    Task<(IEnumerable<WeatherForecast>, string)> GetWeatherForecastFor5DaysAsync(string cityName);
}
