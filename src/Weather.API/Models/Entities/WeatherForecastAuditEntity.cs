namespace Weather.API.Models.Entities;

[Table("WeatherForecastAudit")]
public class WeatherForecastAuditEntity : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string CityName { get; set; }

    [Required]
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedAt { get; set; }

    [MaxLength(1000)]
    public string? Message { get; set; }
}
