namespace Domain.Lobby;

/// <summary>
/// Data produced when a user is granted access to a hub.
/// </summary>
public class UserConnectionContext
{
    /// <summary>
    /// Gets the actual user.
    /// </summary>
    /// <value>User.</value>
    required public User User { get; init; }

    /// <summary>
    /// Gets the lobbyId for the lobby the user is currently connected to.
    /// </summary>
    /// <value>LobbyId.</value>
    required public int LobbyId { get; init; }
}
