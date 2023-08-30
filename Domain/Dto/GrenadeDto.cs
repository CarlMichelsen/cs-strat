using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Grenade.
/// </summary>
[DtoDisplayName("Grenade")]
public class GrenadeDto
{
    /// <summary>
    /// Gets type of grenade.
    /// </summary>
    /// <value>GrenadeType.</value>
    required public string GrenadeType { get; init; }

    /// <summary>
    /// Gets name iof the place this grenade hits.
    /// </summary>
    /// <value>HitLocation name.</value>
    required public string HitLocation { get; init; }

    /// <summary>
    /// Gets name of the place the player should be starting to throw this grenade.
    /// </summary>
    /// <value>Start location name.</value>
    required public string StartLocation { get; init; }

    /// <summary>
    /// Gets ThrowMethod. How should this grenade be thrown.
    /// </summary>
    /// <value>ThrowMethod.</value>
    required public string Method { get; init; }

    /// <summary>
    /// Gets imageUrl or gif url that describes how this grenade should be thrown.
    /// </summary>
    /// <value>Url string.</value>
    required public string ImageUrl { get; init; }
}
