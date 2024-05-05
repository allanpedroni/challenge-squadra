namespace Weather.API.Adapters.OpenWeatherAPI.Dtos;

public class WeatherForecastGetResponseDto
{
    public string cod { get; set; }
    public int message { get; set; }
    public int cnt { get; set; }
    public List[] list { get; set; }
    public City city { get; set; }
}

public class City
{
    public int id { get; set; }
    public string name { get; set; }
    public Coord coord { get; set; }
    public string country { get; set; }
    public int population { get; set; }
    public int timezone { get; set; }
    public int sunrise { get; set; }
    public int sunset { get; set; }
}

public class Coord
{
    public float lat { get; set; }
    public float lon { get; set; }
}

public class List
{
    public int dt { get; set; }
    public Main main { get; set; }
    public Weather[] weather { get; set; }
    [JsonPropertyName("dt_txt")]
    public DateTime dt_txt { get; set; }
}

public class Main
{
    public float temp { get; set; }
    public float feels_like { get; set; }
    public float temp_min { get; set; }
    public float temp_max { get; set; }
    public int pressure { get; set; }
    public int sea_level { get; set; }
    public int grnd_level { get; set; }
    public int humidity { get; set; }
    public float temp_kf { get; set; }
}

public class Weather
{
    public int id { get; set; }
    public string main { get; set; }
    public string description { get; set; }
    public string icon { get; set; }
}
