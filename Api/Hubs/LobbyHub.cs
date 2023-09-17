using BusinessLogic.Mapper;
using Domain.Dto;
using Domain.Lobby;
using Interface.Hubs;
using Interface.Handler;
using Interface.LobbyManagement;
using Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

/// <summary>
/// SignalR hub for real-time communication.
/// </summary>
[Authorize(Roles = "user")]
public class LobbyHub : Hub<ILobbyClient>, ILobbyServer
{
    private readonly ILobbyHubHandler handler;
    private readonly ILogger<LobbyHub> logger;
    private readonly ILobbyAuthService lobbyAuthService;
    private readonly ILobbyManager lobbyManager;

    private UserConnectionContext? context = default;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHub"/> class.
    /// </summary>
    /// <param name="handler">Handle methods.</param>
    /// <param name="logger">For error logging.</param>
    /// <param name="lobbyAuthService">Service for handling authentication and authorization for user joining lobby.</param>
    /// <param name="lobbyManager">Manages lobbies.</param>
    public LobbyHub(
        ILobbyHubHandler handler,
        ILogger<LobbyHub> logger,
        ILobbyAuthService lobbyAuthService,
        ILobbyManager lobbyManager)
    {
        this.handler = handler;
        this.logger = logger;
        this.lobbyAuthService = lobbyAuthService;
        this.lobbyManager = lobbyManager;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1009: Closing parenthesis should be followed by a space", Justification = "Will never be null.")]
    private UserConnectionContext UserContext()
        => (this.Context.Items[nameof(UserConnectionContext)] as UserConnectionContext)!;

    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        var connectionContext = await this.lobbyAuthService
            .Connect(this.Context.User, this.Context.GetHttpContext()?.Request.Query);

        if (connectionContext is null)
        {
            this.Context.Abort();
            return;
        }

        // Add UserContext
        this.Context.Items.Add(nameof(UserConnectionContext), connectionContext);

        var activeLobby = this.lobbyManager
            .GetActiveLobby(connectionContext.LobbyId);
        if (activeLobby is null)
        {
            this.logger.LogCritical("If there is no active lobby at this point I'm throwing a tantrum.");
            this.Context.Abort();
            return;
        }

        var groupName = activeLobby.Id.ToString();

        var connectedUser = activeLobby.Members[connectionContext.User.Id]
            ?? throw new NullReferenceException("This should never ever happen");
        var connectedUserDto = ActiveLobbyMapper.Map(connectedUser);
        await this.Clients.Group(groupName)
            .UserInfo(connectedUserDto);

        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        await this.Clients.Caller
            .Lobby(ActiveLobbyMapper.Map(activeLobby));

        this.context = this.UserContext();
        this.handler.Group = this.Clients.Group(groupName);
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await this.Handle(() => this.handler.OnDisconnectedAsync(exception));
    }

    /// <inheritdoc />
    public async Task DistributeGrenades(List<GrenadeAssignmentDto> grenadeAssignments)
    {
        await this.Handle(() => this.handler.DistributeGrenades(grenadeAssignments));
    }

    /// <inheritdoc />
    public async Task Message(string message)
    {
        await this.Handle(() => this.handler.Message(message));
    }

    private void StructuredHubLog(Exception? exception, LogLevel logLevel)
    {
        var context = this.UserContext();
        this.logger.Log(
            logLevel,
            "{id} -> {user} OnDisconnectedAsync: {}",
            context.User.Id,
            context.User.Name,
            exception);
    }

    private async Task Handle(Func<Task> action)
    {
        try
        {
            await action();
            this.StructuredHubLog(null, LogLevel.Information);
        }
        catch (System.Exception e)
        {
            this.StructuredHubLog(e, LogLevel.Critical);
        }
    }
}
