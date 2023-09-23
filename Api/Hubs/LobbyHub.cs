using BusinessLogic.Mapper;
using Domain.Configuration;
using Domain.Dto;
using Domain.Lobby;
using Domain.Exception;
using Interface.Hubs;
using Interface.LobbyManagement;
using Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

/// <summary>
/// SignalR hub for real-time communication.
/// </summary>
[Authorize(Roles = ApplicationConstants.UserRole)]
public class LobbyHub : Hub<ILobbyClient>, ILobbyServer
{
    private readonly ILogger<LobbyHub> logger;
    private readonly ILobbyAuthService lobbyAuthService;
    private readonly ILobbyStateMachine lobbyStateMachine;
    private readonly ILobbyManager lobbyManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHub"/> class.
    /// </summary>
    /// <param name="logger">For error logging.</param>
    /// <param name="lobbyAuthService">Service for handling authentication and authorization for user joining lobby.</param>
    /// <param name="lobbyStateMachine">Handle methods.</param>
    /// <param name="lobbyManager">Manages lobbies.</param>
    public LobbyHub(
        ILogger<LobbyHub> logger,
        ILobbyAuthService lobbyAuthService,
        ILobbyStateMachine lobbyStateMachine,
        ILobbyManager lobbyManager)
    {
        this.logger = logger;
        this.lobbyAuthService = lobbyAuthService;
        this.lobbyStateMachine = lobbyStateMachine;
        this.lobbyManager = lobbyManager;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1009: Closing parenthesis should be followed by a space", Justification = "Will never be null.")]
    private UserConnectionContext UserContext()
        => (this.Context.Items[ApplicationConstants.LobbyUserConnectionContext] as UserConnectionContext)
            ?? throw new NullReferenceException($"{ApplicationConstants.LobbyUserConnectionContext} was not properly supplied to Context->Items during long lived connection");

    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        await this.Handle(async () =>
        {
            // Add UserContext
            if (!this.Context.GetHttpContext()!.Items.TryGetValue(ApplicationConstants.LobbyUserConnectionContext, out var connectionContextObject))
            {
                this.Context.Abort();
                this.logger.LogCritical("{} was not properly supplied to Context.Items in middleware", ApplicationConstants.LobbyUserConnectionContext);
                return;
            }
            var connectionContext = (UserConnectionContext)connectionContextObject!;
            this.Context.Items[ApplicationConstants.LobbyUserConnectionContext] = connectionContext;

            var activeLobby = this.lobbyManager
                .GetActiveLobby(connectionContext.LobbyId)
                    ?? throw new LobbyHubConnectionException("Lobby should really be present here");

            var groupName = activeLobby.Id.ToString();

            var connectedUser = activeLobby.Members[connectionContext.User.Id]
                ?? throw new LobbyHubConnectionException("The connecting user should be in the lobby they are connecting to here");

            var connectedUserDto = ActiveLobbyMapper.MapToInfo(connectedUser);
            await this.Clients.Group(groupName)
                .UserInfo(connectedUserDto);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.Clients.Caller
                .Lobby(ActiveLobbyMapper.Map(activeLobby));
        });
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await this.Handle(async () =>
        {
            var userContext = this.UserContext();
            if (userContext is null)
            {
                // lobby was closed
                return;
            }

            var activeLobby = this.lobbyManager.GetActiveLobby(userContext.LobbyId);
            if (activeLobby is null)
            {
                return;
            }

            var changedUser = this.lobbyStateMachine
                .UserDisconnected(activeLobby, userContext.User);

            if (changedUser is not null)
            {
                var changedUserDto = ActiveLobbyMapper.MapToInfo(changedUser);
                await this.GetGroup(activeLobby)
                    .UserInfo(changedUserDto);
            }
        });
    }

    /// <inheritdoc />
    public async Task DistributeGrenades(List<GrenadeAssignmentDto> grenadeAssignments)
    {
        await this.Handle(async () =>
        {
            var activeLobby = this.lobbyManager
                .GetActiveLobby(this.UserContext().LobbyId)
                    ?? throw new NullReferenceException("Lobby should always be here");

            var domainGrenadeAssignments = grenadeAssignments.Select(GrenadeMapper.Map);
            var result = this.lobbyStateMachine
                .DistributeGrenades(activeLobby, domainGrenadeAssignments);

            if (result is not null && result.Count() > 0)
            {
                var dtoAssignments = result.Select(GrenadeMapper.Map);
                await this.GetGroup(activeLobby).GrenadeAssignmentsReceived(dtoAssignments);
            }
        });
    }

    private ILobbyClient GetGroup()
    {
        var activeLobby = this.lobbyManager
            .GetActiveLobby(this.UserContext().LobbyId);
        var groupName = activeLobby!.Id.ToString();
        return this.Clients.Group(groupName);
    }

    private ILobbyClient GetGroup(ActiveLobby activeLobby)
    {
        var groupName = activeLobby!.Id.ToString();
        return this.Clients.Group(groupName);
    }

    private async Task Handle(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (System.Exception e)
        {
            this.logger.LogCritical(e, "unhandled exception in LobbyHub");
        }
    }
}
