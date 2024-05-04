namespace Weather.API.Adapters.SqlServer;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

    public DbSet<WeatherForecastAuditEntity> WeatherForecastAuditEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecastAuditEntity>()
            .HasKey(t => new { t.CreatedAt, t.CityName })
            .IsClustered();
    }
}
