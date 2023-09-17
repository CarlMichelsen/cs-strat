using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Wrapper for user that contains useful metadata about the connectionstate of the user.
/// </summary>
[DtoDisplayName("User")]
public class MetaUserDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the user is online (connected).
    /// </summary>
    /// <value>Boolean online value.</value>
    required public bool Online { get; set; }

    /// <summary>
    /// Gets or initiates name for the user.
    /// </summary>
    /// <value>User name.</value>
    required public string Name { get; init; }

    /// <summary>
    /// Gets or sets the grenade the user is excpected to throw.
    /// </summary>
    /// <value>Grenade asignment.</value>
    required public GrenadeDto? GrenadeAssignment { get; set; }
}
