using System.ComponentModel.DataAnnotations;

namespace Domain.Configuration;

/// <summary>
/// Application configuration.
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// Name of the section in configuration.
    /// </summary>
    public const string SectionName = "Swagger";

    /// <summary>
    /// Gets a value indicating whether boolean that indicates whether swagger should be enabled in production.
    /// Swagger is always enabled in development mode.
    /// </summary>
    /// <value>Boolean swagger enabled.</value>
    [Required]
    public bool Enabled { get; init; }
}