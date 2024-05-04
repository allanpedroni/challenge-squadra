namespace Basic.Services.Tests.Services;

public class MoviesServiceTests
{
    private readonly Mock<ITmdbAdapter> _tmdbAdapter = new();
    private readonly Mock<ILoggerFactory> _loggerFactory = new();
    private readonly MoviesService _moviesService;

    public MoviesServiceTests()
    {
        var logger = new Mock<ILogger>();
        _loggerFactory.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

        _moviesService = new MoviesService(_tmdbAdapter.Object, _loggerFactory.Object);
    }

    [Theory]
    [MemberData(nameof(Data.MissingDependencies), MemberType = typeof(Data))]
    public void Constructor_WhenAnyParameterIsNull_ThrowsArgumentNullException(ITmdbAdapter tmdbAdapter, ILoggerFactory loggerFactory)
    {
        // Arrange

        // Act
        Action act = () => new MoviesService(tmdbAdapter, loggerFactory);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async void SearchAsync_WhenMovieSearchIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        SearchMovie movieSearch = null;

        // Act
        Func<Task> act = async () => await _moviesService.SearchAsync(movieSearch);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async void SearchAsync_WhenNoMoviesAreFound_ReturnsEmptyCollection()
    {
        // Arrange
        var movieSearch = new SearchMovie();
        _tmdbAdapter.Setup(adapter => adapter.SearchMoviesAsync(It.IsAny<SearchMovie>())).ReturnsAsync(() => new List<Movie>());

        // Act
        var result = await _moviesService.SearchAsync(movieSearch);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async void SearchAsync_WhenMoviesAreFound_ReturnsMovies()
    {
        // Arrange
        var movieSearch = new SearchMovie();
        var expectedMovies = new List<Movie>() { new() { Title = "Inside Out" }, new() { Title = "Soul" } };
        _tmdbAdapter.Setup(adapter => adapter.SearchMoviesAsync(It.IsAny<SearchMovie>())).ReturnsAsync(() => expectedMovies);

        // Act
        var result = await _moviesService.SearchAsync(movieSearch);

        // Assert
        result.Should().BeEquivalentTo(expectedMovies);
    }

    private static class Data
    {
        public static IEnumerable<object?[]> MissingDependencies
        {
            get
            {
                var logger = new Mock<ILogger>();
                var tmdbAdapter = new Mock<ITmdbAdapter>();
                var loggerFactoryWithoutLogger = new Mock<ILoggerFactory>();
                loggerFactoryWithoutLogger.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns<ILogger?>(null!);

                var loggerFactoryWithLogger = new Mock<ILoggerFactory>();
                loggerFactoryWithLogger.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

                return new List<object?[]>
                    {
                        new object?[] { null, loggerFactoryWithLogger.Object },
                        new object?[] { tmdbAdapter.Object, null  },
                    };
            }
        }
    }
}
