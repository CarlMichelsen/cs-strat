using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Wrapper for user that contains useful metadata about the connectionstate of the user.
/// </summary>
[DtoDisplayName("MetaUser")]
public class MetaUserDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the user is online (connected).
    /// </summary>
    /// <value>Boolean online value.</value>
    required public bool Online { get; set; }

    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <value>User.</value>
    required public UserDto User { get; init; }

    /// <summary>
    /// Gets or sets the grenade the user is excpected to throw.
    /// </summary>
    /// <value>Grenade asignment.</value>
    required public GrenadeDto? GrenadeAsignment { get; set; }
}
