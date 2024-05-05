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
            //TODO: It can be improved!
            //- We can configure AddHttpClient with the base address tagging the name weatherapi.
            var ub = new UriBuilder("https://api.openweathermap.org/data/2.5/forecast");
            ub.Query = $"q={cityName}&appid={_openWeatherConfiguration.ApiKey}&lang={language}&&units=metric";

            var url = ub.Uri.ToString();

            using var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<WeatherForecastGetResponseDto>(json);

                if (data is null)
                {
                    errorMessage = "Failed to deserialize the response from the OpenWeather API.";

                    _logger.LogWarning("Failed to deserialize the response from the OpenWeather API.");
                }
                else
                {
                    _logger.LogInformation("Weather forecast for {CityName} obtained successfully.", cityName);

                    // Group by date to get only one forecast per day
                    var forecasts = data.list
                        .GroupBy(x => x.dt_txt.Date)
                        .Select(x => x.First());

                    weatherForecasts = _mapper.Map<IEnumerable<WeatherForecast>>(forecasts);
                }
            }
            else
            {
                var possibleResponseError = await TryExtractErrorMessageFromResponse(response);

                _logger.LogWarning("Failed to retrieve weather forecast. Error: {ErrorMessage} with status code: {StatusCode} ",
                    possibleResponseError, response.StatusCode);

                errorMessage = $"Failed to retrieve weather forecast with status code: {response.StatusCode} and error message: {possibleResponseError}.";
            }
        }
        //TODO: It can be improved!
        //- We should have a specific exception for the OpenWeather API
        //- According to the OpenWeather docs, here it is the known exceptions
        //- API calls return an error 400, 401, 404, 429, '5xx'
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve weather forecast for {CityName}.", cityName);

            errorMessage = "Failed to retrieve weather forecast.";
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
            _logger.LogError(ex, "Failed to extract the error message from the API response.");
        }

        return errorMessage;
    }
}
