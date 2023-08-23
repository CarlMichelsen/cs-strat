using Domain.Entity;
using Domain.Lobby;
using Microsoft.AspNetCore.Http;

namespace Interface.Service;

/// <summary>
/// Service that handles authentication and authorization when connecting to a hub.
/// </summary>
public interface ILobbyAuthService
{
    /// <summary>
    /// Connect user to lobby.
    /// </summary>
    /// <param name="claimsPrincipal">Claims to get user data from.</param>
    /// <param name="queryCollection">Query collection from hub handshake that contains the lobbyId.</param>
    /// <returns>Nullable UserConnectionContext.</returns>
    Task<UserConnectionContext> Connect(
        System.Security.Claims.ClaimsPrincipal? claimsPrincipal,
        IQueryCollection? queryCollection);
}
