namespace Weather.API.Models.Adapters;

public interface IWeatherForecastAuditReadSqlAdapter
{
    Task<IEnumerable<WeatherForecastAuditEntity>> GetAuditByCityNameAsync(string cityName);
}

