using System.Security.Claims;
using BusinessLogic.Mapper;
using Domain.Configuration;
using Domain.Exception;
using Domain.Lobby;
using Interface.LobbyManagement;
using Interface.Repository;
using Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Service;

/// <inheritdoc />
public class LobbyAuthService : ILobbyAuthService
{
    private readonly ILogger<LobbyAuthService> logger;
    private readonly ILobbyAccessRepository lobbyAccessRepository;
    private readonly ILobbyManager lobbyManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="logger">Logger for logging errors during connection.</param>
    /// <param name="lobbyAccessRepository">Repository for checking lobby access.</param>
    /// <param name="lobbyManager">Manager for lobbies on the server.</param>
    public LobbyAuthService(
        ILogger<LobbyAuthService> logger,
        ILobbyAccessRepository lobbyAccessRepository,
        ILobbyManager lobbyManager)
    {
        this.logger = logger;
        this.lobbyAccessRepository = lobbyAccessRepository;
        this.lobbyManager = lobbyManager;
    }

    /// <inheritdoc />
    public async Task<UserConnectionContext?> Connect(
        ClaimsPrincipal? claimsPrincipal,
        IQueryCollection? queryCollection)
    {
        this.logger.LogCritical("Connect");
        if (claimsPrincipal is null)
        {
            this.LogConnectionError("No claims");
            return default;
        }

        if (queryCollection is null || queryCollection.Count == 0)
        {
            this.LogConnectionError("No query parameters in handshake");
            return default;
        }

        var user = UserMapper.Map(claimsPrincipal);

        var lobbyId = this.GetHandshakeLobbyId(queryCollection);

        var lobbyAccess = await this.lobbyAccessRepository
            .GetLobby(lobbyId);

        if (lobbyAccess is null)
        {
            this.LogConnectionError("Did not find a LobbyAccess record in the database.");
            return default;
        }

        if (!lobbyAccess.Members.Any(u => u == user.Id))
        {
            this.LogConnectionError("User is not a member of the lobby");
            return default;
        }

        var exsistingLobby = this.lobbyManager
            .JoinActiveLobby(lobbyAccess.Id, user)
                ?? this.lobbyManager.InitiateActiveLobby(lobbyAccess, user);

        return new UserConnectionContext
        {
            User = user,
            LobbyId = lobbyAccess.Id,
        };
    }

    private void LogConnectionError(string errorReason)
    {
        this.logger.LogWarning("SignalR connection failed for the following reason: {}", errorReason);
    }

    private string GetHandshakeLobbyId(IQueryCollection queryCollection)
    {
        string? lobbyId = null;

        if (queryCollection.TryGetValue(ApplicationConstants.LobbyIdQueryName, out var lobbyIdStringValue))
        {
            lobbyId = lobbyIdStringValue.FirstOrDefault();
        }

        if (string.IsNullOrWhiteSpace(lobbyId))
        {
            throw new LobbyAuthException($"{ApplicationConstants.LobbyIdQueryName} not found in query parameters");
        }

        return lobbyId;
    }
}
