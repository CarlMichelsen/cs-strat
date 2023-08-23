using System.Security.Claims;
using BusinessLogic.Mapper;
using Domain.Entity;
using Domain.Exception;
using Domain.Lobby;
using Interface.LobbyManagement;
using Interface.Repository;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Service;

/// <inheritdoc />
public class LobbyAuthService : ILobbyAuthService
{
    private const string LobbyIdQueryName = "lobbyId";

    private readonly ILobbyAccessRepository lobbyAccessRepository;
    private readonly ILobbyManager lobbyManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="lobbyAccessRepository">Repository for checking lobby access.</param>
    /// <param name="lobbyManager">Manager for lobbies on the server.</param>
    public LobbyAuthService(
        ILobbyAccessRepository lobbyAccessRepository,
        ILobbyManager lobbyManager)
    {
        this.lobbyAccessRepository = lobbyAccessRepository;
        this.lobbyManager = lobbyManager;
    }

    /// <inheritdoc />
    public async Task<UserConnectionContext> Connect(
        ClaimsPrincipal? claimsPrincipal,
        IQueryCollection? queryCollection)
    {
        if (claimsPrincipal is null)
        {
            throw new LobbyAuthException("No claims");
        }

        if (queryCollection is null || queryCollection.Count == 0)
        {
            throw new LobbyAuthException("No query parameters in handshake");
        }

        var user = UserMapper.Map(claimsPrincipal);

        var lobbyId = this.GetHandshakeLobbyId(queryCollection);

        var lobbyAccess = await lobbyAccessRepository
            .GetLobby(lobbyId);

        if (lobbyAccess is null)
        {
            throw new LobbyAuthException("Did not find a LobbyAccess record in the database.");
        }

        if (!lobbyAccess.Members.Any(u => u == user.Id))
        {
            throw new LobbyAuthException("User is not a member of the lobby");
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

    private string GetHandshakeLobbyId(IQueryCollection queryCollection)
    {
        string? lobbyId = null;

        if (queryCollection.TryGetValue(LobbyIdQueryName, out var lobbyIdStringValue))
        {
            lobbyId = lobbyIdStringValue.FirstOrDefault();
        }

        if (lobbyId is null)
        {
            throw new LobbyAuthException($"{LobbyIdQueryName} not found in query parameters");
        }

        return lobbyId;
    }
}
