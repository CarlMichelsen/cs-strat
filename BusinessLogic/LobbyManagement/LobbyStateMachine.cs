using Domain.Lobby;
using Interface.LobbyManagement;

namespace BusinessLogic.LobbyManagement;

/// <inheritdoc />
public class LobbyStateMachine : ILobbyStateMachine
{
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
