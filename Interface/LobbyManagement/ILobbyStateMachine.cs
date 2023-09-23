using Domain.Lobby;
using Domain.Entity;

namespace Interface.LobbyManagement;

/// <summary>
/// StateMachine for ActiveLobby.
/// </summary>
public interface ILobbyStateMachine
{
    /// <summary>
    /// Create and join a new lobby.
    /// </summary>
    /// <param name="lobbyAccess">Access object to create lobby from.</param>
    /// <param name="joiner">Creator of the lobby.</param>
    /// <returns></returns>
    ActiveLobby CreateActiveLobby(LobbyAccess lobbyAccess, User joiner);

    /// <summary>
    /// Join an already existing lobby.
    /// </summary>
    /// <param name="activeLobby">Lobby to join.</param>
    /// <param name="joiner">Joiner.</param>
    /// <returns></returns>
    ActiveLobby JoinActiveLobby(ActiveLobby activeLobby, User joiner);

    /// <summary>
    /// Alters the active lobby apropriatly and return the now offline metauser.
    /// </summary>
    /// <param name="activeLobby">Lobby to alter.</param>
    /// <param name="user">User that disconnected.</param>
    /// <returns>Offline MetaUser.</returns>
    MetaUser? UserDisconnected(ActiveLobby activeLobby, User user);

    /// <summary>
    ///
    /// </summary>
    /// <param name="activeLobby">The lobby to distribute grenades in.</param>
    /// <param name="grenadeAssignments">Grenades and the userId to receieve them.</param>
    /// <returns>Grenade assignments.</returns>
    IEnumerable<GrenadeAssignment> DistributeGrenades(ActiveLobby activeLobby, IEnumerable<GrenadeAssignment> grenadeAssignments);
}
