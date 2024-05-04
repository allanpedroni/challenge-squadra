using Weather.API.Dtos;
using Weather.API.Models.Adapters;
using Weather.API.Models.Services;

namespace Weather.API.Controllers;

/// <summary>
///     Controlador para a previsão do tempo.
/// </summary>
[ApiController]
[Route("api/[controller]")]
//[Route("api/v{version:apiVersion}/[controller]")]
public class ForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public ForecastController(IWeatherForecastService weatherForecastService)
    {
        this._weatherForecastService = weatherForecastService ?? 
            throw new ArgumentNullException(nameof(weatherForecastService));
    }

    [SwaggerOperation(
        Summary = "Gets the weather forecast for the next 5 days.",
        Description = "Gets the weather forecast for the next 5 days for the city of São Paulo.",
        OperationId = nameof(GetWeatherForecastFor5DaysAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WeatherForecast))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status429TooManyRequests, Type = typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastFor5DaysAsync(
        [FromQuery] WeatherForecastGet weatherForecastGet)
    {
        var weatherForecasts = 
            await _weatherForecastService.GetFor5DaysByCityNameAsync(weatherForecastGet.CityName);

        //Transform the model to DTO response
        //WeatherForecast TO WeatherForecastGetResponse USIGN Mapster

        return weatherForecasts;
    }


   // [SwaggerOperation(
   //    Summary = "Obtém a previsão do tempo para os próximos 5 dias.",
   //    Description = "Obtém a previsão do tempo para os próximos 5 dias para a cidade de São Paulo.",
   //    OperationId = nameof(GetWeatherForecastFor5DaysAsync)
   //)]
   // [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WeatherForecast))]
   // [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
   // [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
   // [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
   // [SwaggerResponse(StatusCodes.Status429TooManyRequests, Type = typeof(ProblemDetails))]
   // [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet("audit")]
    public async Task<IEnumerable<WeatherForecastAudit>> GetWeatherForecastAuditByCityNameAsync(
       [FromQuery] WeatherForecastAuditGet weatherForecastGet)
    {
        var weatherForecasts =
            await _weatherForecastService.GetAuditByCityNameAsync(weatherForecastGet.CityName);

        //Transform the model to DTO response
        //WeatherForecast TO WeatherForecastGetResponse USIGN Mapster

        return weatherForecasts;
    }
}
