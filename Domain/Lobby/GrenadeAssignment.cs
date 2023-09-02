namespace Domain.Lobby;

/// <summary>
/// Grenade assignment.
/// </summary>
public class GrenadeAssignment
{
    /// <summary>
    /// Gets the user that should throw this grenade.
    /// </summary>
    /// <value>UserId.</value>
    required public Guid UserId { get; init; }

    /// <summary>
    /// Gets the grenade to be assigned.
    /// </summary>
    /// <value>Grenade.</value>
    required public Grenade? Assignment { get; init; }
}
