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
    private readonly ILobbyStateMachine lobbyStateMachine;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHub"/> class.
    /// </summary>
    /// <param name="logger">For error logging.</param>
    /// <param name="lobbyAuthService">Service for handling authentication and authorization for user joining lobby.</param>
    /// <param name="lobbyManager">Manages lobbies.</param>
    /// <param name="lobbyStateMachine">Statemachine for altering lobbies.</param>
    public LobbyHub(
        ILogger<LobbyHub> logger,
        ILobbyAuthService lobbyAuthService,
        ILobbyManager lobbyManager,
        ILobbyStateMachine lobbyStateMachine)
    {
        this.logger = logger;
        this.lobbyAuthService = lobbyAuthService;
        this.lobbyManager = lobbyManager;
        this.lobbyStateMachine = lobbyStateMachine;
    }

    private UserConnectionContext UserContext
    {
        get => (this.Context.Items[nameof(UserConnectionContext)] as UserConnectionContext)!;
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
            this.logger.LogInformation("{}: {}", nameof(LobbyAuthException), e.Message);
            this.Context.Abort();
            return;
        }
        catch (System.Exception e)
        {
            this.logger.LogError("Fatal Error {}", e.Message);
            this.Context.Abort();
            return;
        }

        // Add UserContext
        this.Context.Items.Add(nameof(UserConnectionContext), connectionContext);

        var activeLobby = this.lobbyManager.GetActiveLobby(connectionContext.LobbyId);
        if (activeLobby is null)
        {
            this.logger.LogCritical("If there is no active lobby at this point I'm throwing a tantrum.");
            this.Context.Abort();
            return;
        }

        var groupName = activeLobby.Id.ToString();

        var connectedUserDto = ActiveLobbyMapper.Map(activeLobby.Members[connectionContext.User.Id]!);
        await this.Clients.Group(groupName).User(connectedUserDto);

        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        await this.Clients.Group(groupName).Lobby(ActiveLobbyMapper.Map(activeLobby));
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var activeLobby = this.lobbyManager.GetActiveLobby(this.UserContext.LobbyId);
        if (activeLobby is not null)
        {
            var changedUser = this.lobbyStateMachine
                .UserDisconnected(activeLobby, this.UserContext.User);

            if (changedUser is not null)
            {
                var changedUserDto = ActiveLobbyMapper.Map(changedUser);
                await this.Clients.Group(this.UserContext.LobbyId.ToString()).User(changedUserDto);
            }
        }
    }

    /// <inheritdoc />
    public async Task Message(string message)
    {
        await this.Clients.Group(this.UserContext.LobbyId.ToString())
            .MessageReceieved(UserMapper.Map(this.UserContext.User), message);
    }
}
