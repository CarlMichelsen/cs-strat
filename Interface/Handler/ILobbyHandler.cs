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
    Task<ServiceResponse<LobbyDataDto>> CreateLobby(Guid creator);

    /// <summary>
    /// Gets a lobby from human readable id.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Human readable id.</param>
    /// <returns>LobbyDto.</returns>
    Task<ServiceResponse<LobbyDataDto>> GetLobby(string uniqueHumanReadableIdentifier);

    /// <summary>
    /// Joins a lobby.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Lobby to join.</param>
    /// <param name="user">Joining user-id.</param>
    /// <returns>LobbyDto.</returns>
    Task<ServiceResponse<LobbyDataDto>> JoinLobby(string uniqueHumanReadableIdentifier, Guid user);
}