using BusinessLogic.Mapper;
using Domain.Dto;
using Domain.Lobby;
using Interface.Handler;
using Interface.Hubs;
using Interface.LobbyManagement;

namespace BusinessLogic.Handler;

/// <inheritdoc />
public class LobbyHubHandler : ILobbyHubHandler
{
    private readonly ILobbyManager lobbyManager;
    private readonly ILobbyStateMachine lobbyStateMachine;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHubHandler"/> class.
    /// </summary>
    /// <param name="lobbyManager">Manage lobbies.</param>
    /// <param name="lobbyStateMachine">State machine.</param>
    public LobbyHubHandler(
        ILobbyManager lobbyManager,
        ILobbyStateMachine lobbyStateMachine)
    {
        this.lobbyManager = lobbyManager;
        this.lobbyStateMachine = lobbyStateMachine;
    }

    /// <inheritdoc />
    public Func<UserConnectionContext?> UserContext { get; set; } = () => null;

    /// <inheritdoc />
    public ILobbyClient? Group { get; set; } = null;

    /// <inheritdoc />
    public async Task Message(string message)
    {
        await this.Group!
            .MessageReceieved(this.UserContext()!.User.Id, message);
    }

    /// <inheritdoc />
    public async Task DistributeGrenades(List<GrenadeAssignmentDto> grenadeAssignments)
    {
        var userContext = this.UserContext()
            ?? throw new NullReferenceException();
        var activeLobby = this.lobbyManager.GetActiveLobby(userContext.LobbyId);
        if (activeLobby is null)
        {
            return;
        }

        var resultAssignments = this.lobbyStateMachine
            .DistributeGrenades(activeLobby, grenadeAssignments.Select(GrenadeMapper.Map));

        await this.Group!
            .GrenadeAssignmentsReceived(resultAssignments.Select(GrenadeMapper.Map));
    }

    /// <inheritdoc />
    public async Task OnDisconnectedAsync(Exception? exception)
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
            await this.Group!
                .UserInfo(changedUserDto);
        }
    }
}
