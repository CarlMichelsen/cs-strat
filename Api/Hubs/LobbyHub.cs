using BusinessLogic.Mapper;
using Domain.Dto;
using Domain.Lobby;
using Interface.Hubs;
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1009: Closing parenthesis should be followed by a space", Justification = "Will never be null.")]
    private UserConnectionContext UserContext
    {
        get => (this.Context.Items[nameof(UserConnectionContext)] as UserConnectionContext)!;
    }

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
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var activeLobby = this.lobbyManager.GetActiveLobby(this.UserContext.LobbyId);
            if (activeLobby is null)
            {
                return;
            }

            var changedUser = this.lobbyStateMachine
                .UserDisconnected(activeLobby, this.UserContext.User);

            if (changedUser is not null)
            {
                var changedUserDto = ActiveLobbyMapper.Map(changedUser);
                await this.Clients.Group(this.UserContext.LobbyId.ToString())
                    .UserInfo(changedUserDto);
            }
        }
        catch (System.Exception)
        {
            throw;
        }
        finally
        {
            await base.OnDisconnectedAsync(exception);
        }
    }

    /// <inheritdoc />
    public async Task DistributeGrenades(List<GrenadeAssignmentDto> grenadeAssignments)
    {
        var activeLobby = this.lobbyManager.GetActiveLobby(this.UserContext.LobbyId);
        if (activeLobby is null)
        {
            return;
        }

        var resultAssignments = this.lobbyStateMachine
            .DistributeGrenades(activeLobby, grenadeAssignments.Select(GrenadeMapper.Map));

        await this.Clients.Group(this.UserContext.LobbyId.ToString())
            .GrenadeAssignmentsReceived(resultAssignments.Select(GrenadeMapper.Map));
    }

    /// <inheritdoc />
    public async Task Message(string message)
    {
        await this.Clients.Group(this.UserContext.LobbyId.ToString())
            .MessageReceieved(UserMapper.Map(this.UserContext.User), message);
    }
}
