namespace Domain.Lobby;

/// <summary>
/// Wrapper for user that contains useful metadata about the connectionstate of the user.
/// </summary>
public class MetaUser
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
    required public User User { get; init; }

    /// <summary>
    /// Gets or sets the grenade the user is excpected to throw.
    /// </summary>
    /// <value>Grenade asignment.</value>
    required public Grenade? GrenadeAsignment { get; set; }
}
