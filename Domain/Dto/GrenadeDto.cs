using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Grenade.
/// </summary>
[DtoDisplayName("Grenade")]
public class GrenadeDto
{
    /// <summary>
    /// Gets name of the grenade, the name usually references the hitlocation and the type of grenade.
    /// </summary>
    /// <value>Grenade name.</value>
    required public string Name { get; init; }

    /// <summary>
    /// Gets imageUrl or gif url that describes how this grenade should be thrown.
    /// </summary>
    /// <value>Url string.</value>
    required public string ImageUrl { get; init; }
}
