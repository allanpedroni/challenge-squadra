namespace Weather.API.Models;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureMin { get; set; }

    public int TemperatureMax { get; set; }

    //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
