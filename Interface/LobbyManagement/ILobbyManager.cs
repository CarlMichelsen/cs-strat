using Domain.Entity;
using Domain.Lobby;

namespace Interface.LobbyManagement;

/// <summary>
/// LobbyManager, manages all lobbies being hosted on this server.
/// </summary>
public interface ILobbyManager
{
    /// <summary>
    /// Get active lobby.
    /// </summary>
    /// <param name="lobbyId">LobbyId.</param>
    /// <returns>Nullable ActiveLobby.</returns>
    ActiveLobby? GetActiveLobby(int lobbyId);

    /// <summary>
    /// Join active lobby.
    /// </summary>
    /// <param name="lobbyId">LobbyId.</param>
    /// <param name="joiner">User that is joining the lobby.</param>
    /// <returns>Nullable ActiveLobby.</returns>
    ActiveLobby? JoinActiveLobby(int lobbyId, User joiner);

    /// <summary>
    /// Create new active lobby from a LobbyAccess object.
    /// </summary>
    /// <param name="lobbyAccess">LobbyAccess object.</param>
    /// <param name="joiner">User triggering the creation of the active lobby.</param>
    /// <returns>ActiveLobby.</returns>
    ActiveLobby InitiateActiveLobby(LobbyAccess lobbyAccess, User joiner);
}
