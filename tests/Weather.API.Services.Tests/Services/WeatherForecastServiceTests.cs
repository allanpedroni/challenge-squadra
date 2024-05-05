
using System.Globalization;

namespace Weather.API.Tests.Services
{
    public class WeatherForecastServiceTests
    {
        private readonly Mock<IOpenWeatherAPIAdapter> _openWeatherAPIAdapterMock;
        private readonly Mock<IWeatherForecastAuditWriteSqlAdapter> _weatherForecastAuditWriteSqlAdapterMock;
        private readonly Mock<IWeatherForecastAuditReadSqlAdapter> _weatherForecastAuditReadSqlAdapterMock;
        private readonly Mock<ILogger<WeatherForecastService>> _loggerMock;
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastServiceTests()
        {
            _openWeatherAPIAdapterMock = new Mock<IOpenWeatherAPIAdapter>();
            _weatherForecastAuditWriteSqlAdapterMock = new Mock<IWeatherForecastAuditWriteSqlAdapter>();
            _weatherForecastAuditReadSqlAdapterMock = new Mock<IWeatherForecastAuditReadSqlAdapter>();
            _loggerMock = new Mock<ILogger<WeatherForecastService>>();

            _weatherForecastService = new WeatherForecastService(
                _openWeatherAPIAdapterMock.Object,
                _weatherForecastAuditWriteSqlAdapterMock.Object,
                _weatherForecastAuditReadSqlAdapterMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetFor5DaysByCityNameAsync_WithValidCityName_ShouldReturnWeatherForecasts()
        {
            // Arrange
            string cityName = "Seattle";
            var expectedWeatherForecasts = new List<WeatherForecast>
            {
                new WeatherForecast { Date = DateTime.Now.ToString(CultureInfo.InvariantCulture), TemperatureMax= 20, TemperatureMin = 1, Summary = "Sunny" },
                new WeatherForecast { Date = DateTime.Now.AddDays(1).ToString(CultureInfo.InvariantCulture), TemperatureMax= 21, TemperatureMin = 2, Summary = "Cloudy" },
                new WeatherForecast { Date = DateTime.Now.AddDays(2).ToString(CultureInfo.InvariantCulture), TemperatureMax= 22, TemperatureMin = 3, Summary = "Rainy" },
                new WeatherForecast { Date = DateTime.Now.AddDays(3).ToString(CultureInfo.InvariantCulture), TemperatureMax= 23, TemperatureMin = 4, Summary = "Windy" },
                new WeatherForecast { Date = DateTime.Now.AddDays(4).ToString(CultureInfo.InvariantCulture), TemperatureMax= 24, TemperatureMin = 5, Summary = "Partly Cloudy" }
            };
            string errorMessage = null;

            _openWeatherAPIAdapterMock.Setup(x => x.GetWeatherForecastFor5DaysAsync(cityName, "pt_br"))
                .ReturnsAsync((expectedWeatherForecasts, errorMessage));

            // Act
            var result = await _weatherForecastService.GetFor5DaysByCityNameAsync(cityName);

            // Assert
            result.Should().BeEquivalentTo(expectedWeatherForecasts);
            _weatherForecastAuditWriteSqlAdapterMock.Verify(x => x.SaveAuditAsync(cityName, errorMessage), Times.Once);
        }

        [Fact]
        public async Task GetFor5DaysByCityNameAsync_WithNullCityName_ShouldThrowArgumentException()
        {
            // Arrange
            string cityName = null;

            // Act
            Func<Task> act = async () => await _weatherForecastService.GetFor5DaysByCityNameAsync(cityName);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"'{nameof(cityName)}' cannot be null or empty.");
            _weatherForecastAuditWriteSqlAdapterMock.Verify(x => x.SaveAuditAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetFor5DaysByCityNameAsync_WithEmptyCityName_ShouldThrowArgumentException()
        {
            // Arrange
            string cityName = "";

            // Act
            Func<Task> act = async () => await _weatherForecastService.GetFor5DaysByCityNameAsync(cityName);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"'{nameof(cityName)}' cannot be null or empty.");
            _weatherForecastAuditWriteSqlAdapterMock.Verify(x => x.SaveAuditAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetAuditByCityNameAsync_WithValidCityName_ShouldReturnWeatherForecastAuditEntities()
        {
            // Arrange
            string cityName = "Seattle";
            var expectedWeatherForecastAuditEntities = new List<WeatherForecastAuditEntity>
            {
                new WeatherForecastAuditEntity { CityName = cityName, CreatedAt = DateTime.Now, Message = null },
                new WeatherForecastAuditEntity { CityName = cityName, CreatedAt = DateTime.Now.AddDays(-1), Message = "Error" },
                new WeatherForecastAuditEntity { CityName = cityName, CreatedAt = DateTime.Now.AddDays(-2), Message = null }
            };

            _weatherForecastAuditReadSqlAdapterMock.Setup(x => x.GetAuditByCityNameAsync(cityName))
                .ReturnsAsync(expectedWeatherForecastAuditEntities);

            // Act
            var result = await _weatherForecastService.GetAuditByCityNameAsync(cityName);

            // Assert
            result.Should().BeEquivalentTo(expectedWeatherForecastAuditEntities);
        }

        [Fact]
        public async Task GetAuditByCityNameAsync_WithNullCityName_ShouldThrowArgumentException()
        {
            // Arrange
            string cityName = null;

            // Act
            Func<Task> act = async () => await _weatherForecastService.GetAuditByCityNameAsync(cityName);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"'{nameof(cityName)}' cannot be null or empty.");
        }

        [Fact]
        public async Task GetAuditByCityNameAsync_WithEmptyCityName_ShouldThrowArgumentException()
        {
            // Arrange
            string cityName = "";

            // Act
            Func<Task> act = async () => await _weatherForecastService.GetAuditByCityNameAsync(cityName);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"'{nameof(cityName)}' cannot be null or empty.");
        }
    }
}



////namespace Basic.Services.Tests.Services;

////public class WeatherForecastServiceTests
////{
////    private readonly Mock<ITmdbAdapter> _tmdbAdapter = new();
////    private readonly Mock<ILoggerFactory> _loggerFactory = new();
////    private readonly MoviesService _moviesService;

////    public WeatherForecastServiceTests()
////    {
////        var logger = new Mock<ILogger>();
////        _loggerFactory.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

////        _moviesService = new MoviesService(_tmdbAdapter.Object, _loggerFactory.Object);
////    }

////    [Theory]
////    [MemberData(nameof(Data.MissingDependencies), MemberType = typeof(Data))]
////    public void Constructor_WhenAnyParameterIsNull_ThrowsArgumentNullException(ITmdbAdapter tmdbAdapter, ILoggerFactory loggerFactory)
////    {
////        // Arrange

////        // Act
////        Action act = () => new MoviesService(tmdbAdapter, loggerFactory);

////        // Assert
////        act.Should().Throw<ArgumentNullException>();
////    }

////    [Fact]
////    public async void SearchAsync_WhenMovieSearchIsNull_ThrowsArgumentNullException()
////    {
////        // Arrange
////        SearchMovie movieSearch = null;

////        // Act
////        Func<Task> act = async () => await _moviesService.SearchAsync(movieSearch);

////        // Assert
////        await act.Should().ThrowAsync<ArgumentException>();
////    }

////    [Fact]
////    public async void SearchAsync_WhenNoMoviesAreFound_ReturnsEmptyCollection()
////    {
////        // Arrange
////        var movieSearch = new SearchMovie();
////        _tmdbAdapter.Setup(adapter => adapter.SearchMoviesAsync(It.IsAny<SearchMovie>())).ReturnsAsync(() => new List<Movie>());

////        // Act
////        var result = await _moviesService.SearchAsync(movieSearch);

////        // Assert
////        result.Should().BeEmpty();
////    }

////    [Fact]
////    public async void SearchAsync_WhenMoviesAreFound_ReturnsMovies()
////    {
////        // Arrange
////        var movieSearch = new SearchMovie();
////        var expectedMovies = new List<Movie>() { new() { Title = "Inside Out" }, new() { Title = "Soul" } };
////        _tmdbAdapter.Setup(adapter => adapter.SearchMoviesAsync(It.IsAny<SearchMovie>())).ReturnsAsync(() => expectedMovies);

////        // Act
////        var result = await _moviesService.SearchAsync(movieSearch);

////        // Assert
////        result.Should().BeEquivalentTo(expectedMovies);
////    }

////    private static class Data
////    {
////        public static IEnumerable<object?[]> MissingDependencies
////        {
////            get
////            {
////                var logger = new Mock<ILogger>();
////                var tmdbAdapter = new Mock<ITmdbAdapter>();
////                var loggerFactoryWithoutLogger = new Mock<ILoggerFactory>();
////                loggerFactoryWithoutLogger.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns<ILogger?>(null!);

////                var loggerFactoryWithLogger = new Mock<ILoggerFactory>();
////                loggerFactoryWithLogger.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

////                return new List<object?[]>
////                    {
////                        new object?[] { null, loggerFactoryWithLogger.Object },
////                        new object?[] { tmdbAdapter.Object, null  },
////                    };
////            }
////        }
////    }
////}
