using Weather.API.Dtos;
using Weather.API.Models.Services;

namespace Weather.API.Controllers;

/// <summary>
///     Controlador para a previsão do tempo.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class ForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly IMapper _mapper;

    public ForecastController(IWeatherForecastService weatherForecastService,
        IMapper mapper)
    {
        _weatherForecastService = weatherForecastService ??
            throw new ArgumentNullException(nameof(weatherForecastService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [SwaggerOperation(
        Summary = "Gets the weather forecast for the next 5 days.",
        Description = "Gets the weather forecast for the next 5 days for a given city.",
        OperationId = nameof(GetWeatherForecastFor5DaysAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecastGetResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status429TooManyRequests, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet]
    public async Task<IEnumerable<WeatherForecastGetResponse>> GetWeatherForecastFor5DaysAsync(
        [FromQuery] WeatherForecastGet
        weatherForecastGet)
    {
        var weatherForecasts =
            await _weatherForecastService.GetFor5DaysByCityNameAsync(weatherForecastGet.CityName);

        var forecasts = _mapper.Map<IEnumerable<WeatherForecastGetResponse>>(weatherForecasts);

        return forecasts;
    }

    [SwaggerOperation(
        Summary = "Get the audit of weather forecast by city name.",
        Description = "Get the audit of weather forecast for a given city.",
        OperationId = nameof(GetWeatherForecastAuditByCityNameAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecastAuditGetResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet("audit")]
    public async Task<IEnumerable<WeatherForecastAuditGetResponse>> GetWeatherForecastAuditByCityNameAsync(
        [FromQuery] WeatherForecastAuditGet weatherForecastGet)
    {
        var weatherForecasts =
            await _weatherForecastService.GetAuditByCityNameAsync(weatherForecastGet.CityName);

        var audits =_mapper.Map<IEnumerable<WeatherForecastAuditGetResponse>>(weatherForecasts);

        return audits;
    }
}
