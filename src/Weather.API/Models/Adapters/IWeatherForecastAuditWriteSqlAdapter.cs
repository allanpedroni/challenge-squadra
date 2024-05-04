namespace Weather.API.Models.Adapters;

public interface IWeatherForecastAuditWriteSqlAdapter
{
    Task SaveAuditAsync(string cityName, string message);
}

