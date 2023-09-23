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
    /// Receive grenades to throw from the igl.
    /// </summary>
    /// <param name="grenadeAssignments">List of grenade assignments.</param>
    /// <returns>Task.</returns>
    Task GrenadeAssignmentsReceived(IEnumerable<GrenadeAssignmentDto> grenadeAssignments);

    /// <summary>
    /// When new users join or exsisting users change online state or name.
    /// </summary>
    /// <param name="userInfo">Object that contains information about a user that is in the lobby.</param>
    /// <returns>Task.</returns>
    Task UserInfo(UserInfoDto userInfo);
}
