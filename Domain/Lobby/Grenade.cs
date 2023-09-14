namespace Domain.Lobby;

/// <summary>
/// Grenade model.
/// </summary>
public class Grenade
{
    /// <summary>
    /// Gets the team the grenade is relevant for.
    /// </summary>
    /// <value>String team.</value>
    required public string Team { get; init; }

    /// <summary>
    /// Gets description/name of the grenade, the name usually references the hitlocation and the type of grenade.
    /// </summary>
    /// <value>Grenade description.</value>
    required public string Description { get; init; }

    /// <summary>
    /// Gets imageUrl or gif url that describes how this grenade should be thrown.
    /// </summary>
    /// <value>Url string.</value>
    required public string ImageUrl { get; init; }

    /// <summary>
    /// Gets value that indicates whether the grenade is a jumpthrow.
    /// </summary>
    /// <value>Jumpthrow boolean.</value>
    required public bool Jumpthrow { get; init; }
}
