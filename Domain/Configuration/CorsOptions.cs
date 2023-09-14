using System.ComponentModel.DataAnnotations;

namespace Domain.Configuration;

/// <summary>
/// CORS configuration options.
/// </summary>
public class CorsOptions
{
    /// <summary>
    /// Name of the section in configuration.
    /// </summary>
    public const string SectionName = "Cors";

    /// <summary>
    /// Gets string of comma-separated urls Cors should recognize as origins.
    /// </summary>
    /// <value>Comma-separated origin url strings.</value>
    [Required]
    required public string WithOrigins { get; init; }
}
