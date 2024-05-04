using Weather.API.Models.Services;

namespace Weather.API.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IOpenWeatherAPIAdapter _openWeatherAPIAdapter;
    private readonly IWeatherForecastAuditWriteSqlAdapter _weatherForecastAuditWriteSqlAdapter;
    private readonly IWeatherForecastAuditReadSqlAdapter _weatherForecastAuditReadSqlAdapter;
    private readonly ILogger<WeatherForecastService> _logger;
    private readonly IMapper _mapper;

    public WeatherForecastService(IOpenWeatherAPIAdapter openWeatherAPIAdapter,
        IWeatherForecastAuditWriteSqlAdapter weatherForecastAuditWriteSqlAdapter,
        IWeatherForecastAuditReadSqlAdapter weatherForecastAuditReadSqlAdapter,
        ILogger<WeatherForecastService> logger,
        IMapper mapper)
    {
        _openWeatherAPIAdapter = openWeatherAPIAdapter ?? throw new ArgumentNullException(nameof(openWeatherAPIAdapter));
        _weatherForecastAuditWriteSqlAdapter = weatherForecastAuditWriteSqlAdapter ?? throw new ArgumentNullException(nameof(weatherForecastAuditWriteSqlAdapter));
        _weatherForecastAuditReadSqlAdapter = weatherForecastAuditReadSqlAdapter ?? throw new ArgumentNullException(nameof(weatherForecastAuditReadSqlAdapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<WeatherForecast>> GetFor5DaysByCityNameAsync(string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            throw new ArgumentException($"'{nameof(cityName)}' cannot be null or empty.", nameof(cityName));
        }

        _logger.LogDebug("Obtendo a previsão do tempo para a cidade de {CityName}.", cityName);

        var (weatherForecasts, errorMessage) =
            await _openWeatherAPIAdapter.GetWeatherForecastFor5DaysAsync(cityName);

        await _weatherForecastAuditWriteSqlAdapter.SaveAuditAsync(cityName, errorMessage);

        return weatherForecasts;

    }

    public async Task<IEnumerable<WeatherForecastAudit>> GetAuditByCityNameAsync(string cityName)
    {
        var audits =
            await _weatherForecastAuditReadSqlAdapter.GetAuditByCityNameAsync(cityName);

        var weatherForecastAudits =
            _mapper.Map<IEnumerable<WeatherForecastAudit>>(audits);

        return weatherForecastAudits;
    }

}
