namespace Domain.Lobby;

/// <summary>
/// Wrapper for user that contains useful metadata about the connectionstate of the user.
/// </summary>
public class MetaUser
{
    /// <summary>
    /// Gets or sets online (connected) state of the user.
    /// </summary>
    /// <value>Boolean online value.</value>
    required public bool Online { get; set; }

    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <value>User.</value>
    required public User User { get; init; }
}
