namespace Weather.API.Dtos;

/// <summary>
/// Represents a weather forecast response for a specific city.
/// </summary>
/// <param name="CityName"> City name</param>
/// <param name="CreatedAt"> Date and time of the audit</param>
/// <param name="Message"> Message (it could be an error message)</param>
public record WeatherForecastAuditGetResponse(string CityName, DateTime CreatedAt, string? Message);
