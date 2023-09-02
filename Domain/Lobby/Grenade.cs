namespace Domain.Lobby;

/// <summary>
/// Grenade model.
/// </summary>
public class Grenade
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
