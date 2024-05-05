using Weather.API.Models.Services;

namespace Weather.API.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IOpenWeatherAPIAdapter _openWeatherAPIAdapter;
    private readonly IWeatherForecastAuditWriteSqlAdapter _weatherForecastAuditWriteSqlAdapter;
    private readonly IWeatherForecastAuditReadSqlAdapter _weatherForecastAuditReadSqlAdapter;
    private readonly ILogger<WeatherForecastService> _logger;

    public WeatherForecastService(IOpenWeatherAPIAdapter openWeatherAPIAdapter,
        IWeatherForecastAuditWriteSqlAdapter weatherForecastAuditWriteSqlAdapter,
        IWeatherForecastAuditReadSqlAdapter weatherForecastAuditReadSqlAdapter,
        ILogger<WeatherForecastService> logger)
    {
        _openWeatherAPIAdapter = openWeatherAPIAdapter ?? throw new ArgumentNullException(nameof(openWeatherAPIAdapter));
        _weatherForecastAuditWriteSqlAdapter = weatherForecastAuditWriteSqlAdapter ?? throw new ArgumentNullException(nameof(weatherForecastAuditWriteSqlAdapter));
        _weatherForecastAuditReadSqlAdapter = weatherForecastAuditReadSqlAdapter ?? throw new ArgumentNullException(nameof(weatherForecastAuditReadSqlAdapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<WeatherForecast>> GetFor5DaysByCityNameAsync(string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            throw new ArgumentException($"'{nameof(cityName)}' cannot be null or empty.");
        }

        _logger.LogDebug("Obtendo a previsão do tempo para a cidade de {CityName}.", cityName);

        var (weatherForecasts, errorMessage) =
            await _openWeatherAPIAdapter.GetWeatherForecastFor5DaysAsync(cityName);

        await _weatherForecastAuditWriteSqlAdapter.SaveAuditAsync(cityName, errorMessage);

        return weatherForecasts;

    }

    public async Task<IEnumerable<WeatherForecastAuditEntity>> GetAuditByCityNameAsync(string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            throw new ArgumentException($"'{nameof(cityName)}' cannot be null or empty.");
        }

        return
            await _weatherForecastAuditReadSqlAdapter.GetAuditByCityNameAsync(cityName);
    }
}
