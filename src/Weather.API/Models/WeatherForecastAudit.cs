namespace Weather.API.Models;

public class WeatherForecastAudit
{
    public string CityName { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Message { get; set; }
}
