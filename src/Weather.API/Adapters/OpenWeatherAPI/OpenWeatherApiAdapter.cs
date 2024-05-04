namespace Weather.API.Adapters.OpenWeatherAPI;

[ApiVersion(1.0)]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class OpenWeatherApiAdapter : IOpenWeatherAPIAdapter
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherConfiguration _openWeatherConfiguration;
    private readonly ILogger<OpenWeatherApiAdapter> _logger;

    public OpenWeatherApiAdapter(OpenWeatherConfiguration openWeatherConfiguration,
        IHttpClientFactory httpClientFactory, ILogger<OpenWeatherApiAdapter> logger)
    {
        _httpClient = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _openWeatherConfiguration = openWeatherConfiguration;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(IEnumerable<WeatherForecast>, string)> GetWeatherForecastFor5DaysAsync(string cityName)
    {
        var weatherForecasts = Enumerable.Empty<WeatherForecast>();
        string errorMessage = string.Empty;

        try
        {

            var ub = new UriBuilder("https://openweathermap.org/data/2.5/onecall");
            ub.Query = $"lat=-23.553721003637605&lon=-46.6571524012837&units=metric&appid={_openWeatherConfiguration.ApiKey}&lang=pt_br";

            //TODO: use UrlBuilder
            //string url = $"https://openweathermap.org/data/2.5/onecall?&&lat=-23.553721003637605&lon=-46.6571524012837&units=metric&appid={_openWeatherConfiguration.ApiKey}&lang=pt_br";

            var url = ub.Uri.ToString();

            //REF: https://openweathermap.org/forecast5#geo5

            using var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<WeatherForecastGetResponse>(json);

                if (data is null)
                {
                    errorMessage = "Falha ao deserializar a resposta da API do OpenWeather.";

                    _logger.LogWarning("Falha ao deserializar a resposta da API do OpenWeather.");
                }
                else
                {
                    _logger.LogInformation("Previsão do tempo para {NomeCidade} obtida com sucesso.", cityName);

                    weatherForecasts = data.daily.Select(d => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(d.dt).DateTime),
                        TemperatureMin = d.temp.min < 0 ? 0 : (int)d.temp.min,
                        TemperatureMax = d.temp.max < 0 ? 0 : (int)d.temp.max,
                        Summary = d.weather.FirstOrDefault()?.description ?? string.Empty,
                    });
                }
            }
            else
            {
                var possibleResponseError = await TryExtractErrorMessageFromResponse(response);

                _logger.LogWarning("Falha ao obter a previsão do tempo. Error: {ErrorMessage} com código de status: {StatusCode} ",
                    possibleResponseError, response.StatusCode);

                errorMessage = $"Falha ao obter a previsão do tempo com código de status: {response.StatusCode} e mensagem de error: {possibleResponseError}.";
            }
        }
        //TODO: according to the OpenWeather docs, here it is the known exceptions
        //API calls return an error 400, 401, 404, 429, '5xx'
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao obter a previsão do tempo para {NomeCidade}.", cityName);

            errorMessage = "Falha ao obter a previsão do tempo.";
        }

        return (weatherForecasts, errorMessage);
    }

    private async Task<string> TryExtractErrorMessageFromResponse(HttpResponseMessage response)
    {
        string errorMessage = string.Empty;

        try
        {
            errorMessage = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao extrair a mensagem de erro da resposta da API.");
        }

        return errorMessage;
    }
}
