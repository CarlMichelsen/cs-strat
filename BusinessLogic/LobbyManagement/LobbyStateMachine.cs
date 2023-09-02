using Domain.Lobby;
using Interface.LobbyManagement;

namespace BusinessLogic.LobbyManagement;

/// <inheritdoc />
public class LobbyStateMachine : ILobbyStateMachine
{
    /// <inheritdoc />
    public IEnumerable<GrenadeAssignment> DistributeGrenades(ActiveLobby activeLobby, IEnumerable<GrenadeAssignment> grenadeAssignments)
    {
        var successfulAssignments = new List<GrenadeAssignment>();
        foreach (var assignment in grenadeAssignments)
        {
            if (activeLobby.Members.TryGetValue(assignment.UserId, out var member))
            {
                member.GrenadeAsignment = assignment.Assignment;
                successfulAssignments.Add(assignment);
            }
        }

        return successfulAssignments;
    }

    /// <inheritdoc />
    public MetaUser? UserDisconnected(ActiveLobby activeLobby, User user)
    {
        if (activeLobby.Members.TryGetValue(user.Id, out var metaUser))
        {
            metaUser.Online = false;
            return metaUser;
        }

        return default;
    }
}
