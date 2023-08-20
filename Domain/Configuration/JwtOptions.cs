using System.ComponentModel.DataAnnotations;

namespace Domain.Configuration;

/// <summary>
/// Jwt configuration options.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Name of the section in configuration.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Gets or initiates name of the cookie the browser sends.
    /// </summary>
    /// <value>Cookie name string.</value>
    [Required]
    required public string CookieName { get; init; }

    /// <summary>
    /// Gets or initiates JWT issuer.
    /// </summary>
    /// <value>Issuer string.</value>
    [Required]
    required public string Issuer { get; init; }

    /// <summary>
    /// Gets or initiates JWT audience.
    /// </summary>
    /// <value>Audience string.</value>
    [Required]
    required public string Audience { get; init; }

    /// <summary>
    /// Gets or initiates JWT Time To Live Days.
    /// </summary>
    /// <value>Time to live int.</value>
    [Required]
    [Range(1, 31)]
    required public int TTLDays { get; init; }
    
    /// <summary>
    /// Gets or initiates JWT key.
    /// </summary>
    /// <value>Key string.</value>
    [Required]
    required public string Key { get; init; }
}