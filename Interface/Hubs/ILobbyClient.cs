using Domain.Attribute;
using Domain.Dto;

namespace Interface.Hubs;

/// <summary>
/// SignalR methods that are present on the client that can be triggered from the server.
/// </summary>
[DtoDisplayName("ClientSignalRMethods")]
public interface ILobbyClient
{
    /// <summary>
    /// Source of truth for lobby.
    /// </summary>
    /// <param name="lobby">Lobby object.</param>
    /// <returns>Task.</returns>
    Task Lobby(ActiveLobbyDto lobby);

    /// <summary>
    /// Receieve message from another member of the lobby.
    /// </summary>
    /// <param name="sender">Userid for sender.</param>
    /// <param name="message">String message.</param>
    /// <returns>Task.</returns>
    Task MessageReceieved(Guid sender, string message);

    /// <summary>
    /// Receive grenades to throw from the igl.
    /// </summary>
    /// <param name="grenadeAssignments">List of grenade assignments.</param>
    /// <returns>Task.</returns>
    Task GrenadeAssignmentsReceived(IEnumerable<GrenadeAssignmentDto> grenadeAssignments);

    /// <summary>
    /// When new users join or exsisting users change online state or name.
    /// </summary>
    /// <param name="metaUser">Wrapper object with a user in it.</param>
    /// <returns>Task.</returns>
    Task UserInfo(MetaUserDto metaUser);
}
