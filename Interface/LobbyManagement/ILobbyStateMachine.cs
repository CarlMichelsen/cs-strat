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
    /// <param name="activeLobby"></param>
    /// <param name="grenades"></param>
    /// <returns></returns>
    Dictionary<Guid, Grenade> DistributeGrenades(ActiveLobby activeLobby, List<Grenade> grenades); // TODO: add return type and model grenade implementation.
}
