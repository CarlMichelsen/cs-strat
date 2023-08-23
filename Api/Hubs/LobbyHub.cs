using Interface.Hubs;
using Interface.Service;
using Interface.LobbyManagement;
using Domain.Lobby;
using Domain.Exception;
using BusinessLogic.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

/// <summary>
/// SignalR hub for real-time communication.
/// </summary>
[Authorize(Roles = "user")]
public class LobbyHub : Hub<ILobbyClient>, ILobbyServer
{
    private readonly ILogger<LobbyHub> logger;
    private readonly ILobbyAuthService lobbyAuthService;
    private readonly ILobbyManager lobbyManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHub"/> class.
    /// </summary>
    /// <param name="logger">For error logging.</param>
    /// <param name="lobbyAuthService">Service for handling authentication and authorization for user joining lobby.</param>
    /// <param name="lobbyManager">For error logging.</param>
    public LobbyHub(
        ILogger<LobbyHub> logger,
        ILobbyAuthService lobbyAuthService,
        ILobbyManager lobbyManager)
    {
        this.logger = logger;
        this.lobbyAuthService = lobbyAuthService;
        this.lobbyManager = lobbyManager;
    }

    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        UserConnectionContext connectionContext;

        try
        {
            connectionContext = await this.lobbyAuthService
                .Connect(this.Context.User, this.Context.GetHttpContext()?.Request.Query);
        }
        catch (LobbyAuthException e)
        {
            this.logger.LogInformation("A user attempted to connect to the lobby but got the following {}: {}",
                nameof(LobbyAuthException), e.Message);
            this.Context.Abort();
            return;
        }
        catch (System.Exception e)
        {
            this.logger.LogError("An unhandled exception was thrown attempting to connect with the message {}",
                e.Message);
            this.Context.Abort();
            return;
        }

        Context.Items.Add(nameof(UserConnectionContext), connectionContext);

        var activeLobby = this.lobbyManager.GetActiveLobby(connectionContext.LobbyId);
        if (activeLobby is null)
        {
            this.logger.LogCritical("If there is no active lobby at this point I'm throwing a tantrum.");
            this.Context.Abort();
            return;
        }

        var groupName = activeLobby.Id.ToString();

        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        await this.Clients.Group(groupName).Lobby(ActiveLobbyMapper.Map(activeLobby));
    }

    /// <inheritdoc />
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return Task.CompletedTask;
    }
}
