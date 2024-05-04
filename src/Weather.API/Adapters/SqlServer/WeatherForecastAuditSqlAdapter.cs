namespace Weather.API.Adapters.SqlServer;

public class WeatherForecastAuditSqlAdapter : 
    IWeatherForecastAuditWriteSqlAdapter, IWeatherForecastAuditReadSqlAdapter
{
    private readonly WeatherDbContext _weatherDbContext;
    private readonly ILogger<WeatherForecastAuditSqlAdapter> _logger;

    public WeatherForecastAuditSqlAdapter(
        WeatherDbContext weatherDbContext,
        ILogger<WeatherForecastAuditSqlAdapter> logger)
    {
        _weatherDbContext = weatherDbContext ?? 
            throw new ArgumentNullException(nameof(weatherDbContext));
        _logger = logger ?? 
            throw new ArgumentNullException(nameof(logger));
    }

    public async Task SaveAuditAsync(string cityName, string message)
    {
        try
        {
            _weatherDbContext.WeatherForecastAuditEntities.Add(new WeatherForecastAuditEntity
            {
                Id = Guid.NewGuid(),
                CityName = cityName,
                CreatedAt = DateTime.Now,
                Message = message
            });

            await _weatherDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar o log de auditoria.");
        }
    }

    public async Task<IEnumerable<WeatherForecastAuditEntity>> GetAuditByCityNameAsync(string cityName)
    {
        return await _weatherDbContext.WeatherForecastAuditEntities
            .Where(w => w.CityName == cityName)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }
}
