using BusinessLogic.Mapper;
using Domain.Entity;
using Domain.Lobby;
using Interface.LobbyManagement;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.LobbyManagement;

/// <inheritdoc />
public class LobbyStateMachine : ILobbyStateMachine
{
    private readonly ILogger<LobbyStateMachine> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyStateMachine"/> class.
    /// </summary>
    /// <param name="logger">For logging events.</param>
    public LobbyStateMachine(
        ILogger<LobbyStateMachine> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc />
    public ActiveLobby CreateActiveLobby(LobbyAccess lobbyAccess, User joiner)
    {
        this.logger.LogInformation("{id} -> ({name})  is instantiating lobby {lobbyid}", joiner.Id, joiner.Name, lobbyAccess.UniqueHumanReadableIdentifier);

        var activeLobby = ActiveLobbyMapper.Map(lobbyAccess);

        var metaUser = new MetaUser
        {
            Online = true,
            User = joiner,
            GrenadeAssignment = default,
        };
        activeLobby.Members.Add(joiner.Id, metaUser);

        return activeLobby;
    }

    /// <inheritdoc />
    public ActiveLobby JoinActiveLobby(ActiveLobby activeLobby, User joiner)
    {
        this.logger.LogInformation("{id} -> ({name})  is connecting to lobby {lobbyid}", joiner.Id, joiner.Name, activeLobby.UniqueHumanReadableIdentifier);

        if (activeLobby.Members.TryGetValue(joiner.Id, out var metaUser))
        {
            metaUser.Online = true;
        }
        else
        {
            activeLobby.Members.Add(joiner.Id, new MetaUser
            {
                Online = true,
                User = joiner,
                GrenadeAssignment = default,
            });
        }

        return activeLobby;
    }

    /// <inheritdoc />
    public IEnumerable<GrenadeAssignment> DistributeGrenades(ActiveLobby activeLobby, IEnumerable<GrenadeAssignment> grenadeAssignments)
    {
        var successfulAssignments = new List<GrenadeAssignment>();
        foreach (var assignment in grenadeAssignments)
        {
            if (activeLobby.Members.TryGetValue(assignment.UserId, out var member))
            {
                member.GrenadeAssignment = assignment.Assignment;
                successfulAssignments.Add(assignment);
            }
        }

        this.logger.LogInformation("{} grenades were assigned in lobby {}", successfulAssignments.Count, activeLobby.UniqueHumanReadableIdentifier);

        return successfulAssignments;
    }

    /// <inheritdoc />
    public MetaUser? UserDisconnected(ActiveLobby activeLobby, User user)
    {
        this.logger.LogInformation("{id} -> ({name}) disconnected from lobby {}", user.Id, user.Name, activeLobby.UniqueHumanReadableIdentifier);

        if (activeLobby.Members.TryGetValue(user.Id, out var metaUser))
        {
            metaUser.Online = false;
            return metaUser;
        }

        return default;
    }
}
