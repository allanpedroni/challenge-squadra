namespace Weather.API.Models;

public class WeatherForecastGetResponse
{
    public string Date { get; set; }

    public int TemperatureMin { get; set; }

    public int TemperatureMax { get; set; }

    public string? Summary { get; set; }

    public string? Icon { get; set; } = string.Empty;
}
