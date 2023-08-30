using BusinessLogic.Mapper;
using Domain.Entity;
using Domain.Lobby;
using Interface.LobbyManagement;

namespace BusinessLogic.LobbyManagement;

/// <inheritdoc />
public class LobbyManager : ILobbyManager
{
    private static Dictionary<int, ActiveLobby> activeLobbies = new();

    /// <inheritdoc />
    public ActiveLobby? GetActiveLobby(int lobbyId)
    {
        if (activeLobbies.TryGetValue(lobbyId, out var activeLobby))
        {
            return activeLobby;
        }

        return default;
    }

    /// <inheritdoc />
    public ActiveLobby? JoinActiveLobby(int lobbyId, User joiner)
    {
        var lobby = this.GetActiveLobby(lobbyId);

        if (lobby is null)
        {
            return default;
        }

        if (lobby.Members.TryGetValue(joiner.Id, out var metaUser))
        {
            metaUser.Online = true;
        }
        else
        {
            lobby.Members.Add(joiner.Id, new MetaUser
            {
                Online = true,
                User = joiner,
                GrenadeAsignment = default,
            });
        }

        return lobby;
    }

    /// <inheritdoc />
    public ActiveLobby InitiateActiveLobby(LobbyAccess lobbyAccess, User joiner)
    {
        var activeLobby = ActiveLobbyMapper.Map(lobbyAccess);

        var metaUser = new MetaUser
        {
            Online = true,
            User = joiner,
            GrenadeAsignment = default,
        };
        activeLobby.Members.Add(joiner.Id, metaUser);

        activeLobbies.Add(activeLobby.Id, activeLobby);

        return activeLobby;
    }
}
