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
                var changedUserDto = ActiveLobbyMapper.Map(changedUser);
                await this.GetGroup(activeLobby)
                    .UserInfo(changedUserDto);
            }
        });
    }

    /// <inheritdoc />
    public async Task DistributeGrenades(List<GrenadeAssignmentDto> grenadeAssignments)
    {
        this.logger.LogInformation("Hello==?=");
        await this.Handle(async () =>
        {
            var activeLobby = this.lobbyManager
                .GetActiveLobby(this.UserContext().LobbyId)
                    ?? throw new NullReferenceException("Lobby should always be here");

            this.logger.LogInformation("{}", grenadeAssignments.First().UserId);

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

    /// <inheritdoc />
    public async Task Message(string message)
    {
        await this.Handle(async () =>
        {
            await GetGroup()
                .MessageReceieved(this.UserContext()!.User.Id, message);
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

    private void StructuredHubLog(Exception? exception, string description, LogLevel logLevel)
    {
        var context = this.UserContext();
        this.logger.Log(
            logLevel,
            "{id} -> {user} >{description}< {exception}",
            context.User.Id,
            context.User.Name,
            description,
            exception);
    }

    private async Task Handle(Func<Task> action)
    {
        try
        {
            await action();
            this.StructuredHubLog(null, "action", LogLevel.Information);
        }
        catch (System.Exception e)
        {
            this.StructuredHubLog(e, "unhandled exception", LogLevel.Critical);
        }
    }
}
