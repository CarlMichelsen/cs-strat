using Domain.Lobby;

namespace Interface.LobbyManagement;

/// <summary>
/// StateMachine for ActiveLobby.
/// </summary>
public interface ILobbyStateMachine
{
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
