using System.Diagnostics.CodeAnalysis;

namespace Weather.API.Extensions;

[ExcludeFromCodeCoverage]
public static class CommonExtensions
{
    public static WebApplicationBuilder AddBaseServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        builder.Services.AddProblemDetails()
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services
            .AddSingleton(WebApiMapping.Default())
            .AddSingleton(InfrastructureMapping.Default())
            .AddScoped<IMapper, ServiceMapper>();

        return builder;
    }

    public static WebApplicationBuilder AddEntityFramework(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<WeatherDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default") ??
                                   throw new InvalidOperationException("The connection string Default is null");

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseSqlServer(connectionString, c =>
            {
                c.CommandTimeout(30);
                c.EnableRetryOnFailure(3);
                c.MigrationsAssembly("Weather.API");
            });

            if (builder.Environment.IsDevelopment())
            {
                options.LogTo(Log.Debug);
                options.EnableDetailedErrors();
            }
        });

        return builder;
    }

    public static async Task AddMigrationsAsync(this WebApplication app, WebApplicationBuilder builder)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

        await context.Database.MigrateAsync();

        if (builder.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGenNewtonsoftSupport()
            .AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new()
                {
                    Title = "Weather API",
                    Version = "v1"
                });
            })
            .AddApiVersioning(
                options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                })
            .AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

        return builder;
    }

    public static WebApplication AddSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(
            options =>
            {
                options.DocumentTitle = "Weather API";

                foreach (var description in app.DescribeApiVersions())
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);

                    options.DisplayRequestDuration();
                }
            });
        }

        return app;
    }
}
