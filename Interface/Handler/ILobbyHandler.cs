using Domain.Dto;

namespace Interface.Handler;

/// <summary>
/// Handler for lobby endpoints.
/// </summary>
public interface ILobbyHandler
{
    /// <summary>
    /// Creates a lobby and returns it.
    /// </summary>
    /// <param name="creator">Guid id of the creator.</param>
    /// <returns>LobbyDto.</returns>
    Task<ServiceResponse<string>> CreateLobby(Guid creator);

    /// <summary>
    /// Joins a lobby.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Lobby to join.</param>
    /// <param name="user">Joining user-id.</param>
    /// <returns>LobbyDto.</returns>
    Task<ServiceResponse<string>> JoinLobby(string uniqueHumanReadableIdentifier, Guid user);
}
