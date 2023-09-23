using Domain.Entity;
using Domain.Lobby;
using Interface.LobbyManagement;

namespace BusinessLogic.LobbyManagement;

/// <inheritdoc />
public class LobbyManager : ILobbyManager
{
    private readonly ILobbyStateMachine lobbyStateMachine;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyManager"/> class.
    /// </summary>
    /// <param name="lobbyStateMachine">Statemachine for mutating ActiveLobby.</param>
    public LobbyManager(
        ILobbyStateMachine lobbyStateMachine)
    {
        this.lobbyStateMachine = lobbyStateMachine;
    }

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

        return this.lobbyStateMachine.JoinActiveLobby(lobby, joiner);
    }

    /// <inheritdoc />
    public ActiveLobby InitiateActiveLobby(LobbyAccess lobbyAccess, User joiner)
    {
        var activeLobby = this.lobbyStateMachine.CreateActiveLobby(lobbyAccess, joiner);
        activeLobbies.Add(activeLobby.Id, activeLobby);
        return activeLobby;
    }
}
