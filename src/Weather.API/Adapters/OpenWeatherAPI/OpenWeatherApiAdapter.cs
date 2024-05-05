namespace Weather.API.Adapters.OpenWeatherAPI;

public class OpenWeatherApiAdapter : IOpenWeatherAPIAdapter
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherConfiguration _openWeatherConfiguration;
    private readonly ILogger<OpenWeatherApiAdapter> _logger;
    private readonly IMapper _mapper;

    public OpenWeatherApiAdapter(OpenWeatherConfiguration openWeatherConfiguration,
        IHttpClientFactory httpClientFactory, ILogger<OpenWeatherApiAdapter> logger,
        IMapper mapper)
    {
        _httpClient = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _openWeatherConfiguration = openWeatherConfiguration;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<(IEnumerable<WeatherForecast>, string)> GetWeatherForecastFor5DaysAsync(string cityName, string language = "pt_br")
    {
        var weatherForecasts = Enumerable.Empty<WeatherForecast>();
        string errorMessage = string.Empty;

        try
        {
            var ub = new UriBuilder("https://api.openweathermap.org/data/2.5/forecast");
            ub.Query = $"q={cityName}&appid={_openWeatherConfiguration.ApiKey}&lang={language}&&units=metric&cnt=5";

            var url = ub.Uri.ToString();

            using var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<WeatherForecastGetResponseDto>(json);

                if (data is null)
                {
                    errorMessage = "Falha ao deserializar a resposta da API do OpenWeather.";

                    _logger.LogWarning("Falha ao deserializar a resposta da API do OpenWeather.");
                }
                else
                {
                    _logger.LogInformation("Previsão do tempo para {NomeCidade} obtida com sucesso.", cityName);

                    weatherForecasts = _mapper.Map<IEnumerable<WeatherForecast>>(data.list);
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
