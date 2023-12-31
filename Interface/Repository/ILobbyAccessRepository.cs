using Domain.Entity;

namespace Interface.Repository;

/// <summary>
/// Repository for creating and joining lobbies.
/// </summary>
public interface ILobbyAccessRepository
{
    /// <summary>
    /// Create lobby.
    /// </summary>
    /// <param name="lobby">Lobby.</param>
    /// <returns>The created lobby.</returns>
    Task<LobbyAccess> CreateLobby(LobbyAccess lobby);

    /// <summary>
    /// Get a lobby from uniqueHumanReadableIdentifier.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Identifier for lobby.</param>
    /// <returns>The created lobby.</returns>
    Task<LobbyAccess?> GetLobby(string uniqueHumanReadableIdentifier);

    /// <summary>
    /// Join a lobby by uniqueHumanReadableIdentifier.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Lobby identifier.</param>
    /// <param name="user">Joining user guid.</param>
    /// <returns>Lobby, post joining.</returns>
    Task<LobbyAccess?> JoinLobby(string uniqueHumanReadableIdentifier, Guid user);

    /// <summary>
    /// Set UniqueHumanReadableIdentifier to null, allowing new lobbies to use it.
    /// There is a limited amount of unique human readable identifiers so it is important to clear some old ones up for use again.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Get lobby by uniqueHumanReadableIdentifier to clear uniqueHumanReadableIdentifier.</param>
    /// <returns>Lobby post removal of uniqueHumanReadableIdentifier.</returns>
    Task<LobbyAccess?> ClearUniqueHumanReadableIdentifier(string uniqueHumanReadableIdentifier);
}
