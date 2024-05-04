namespace Weather.API.Dtos;

/// <summary>
/// Represents a weather forecast request for a specific city.
/// </summary>
/// <param name="CityName">City name</param>
public record WeatherForecastAuditGet(string CityName);
