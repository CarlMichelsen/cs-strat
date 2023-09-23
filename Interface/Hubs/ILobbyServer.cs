using Domain.Attribute;
using Domain.Dto;

namespace Interface.Hubs;

/// <summary>
/// SignalR methods that are present on the server that can be triggered from the client.
/// </summary>
[DtoDisplayName("ServerSignalRMethods")]
public interface ILobbyServer
{
    /// <summary>
    /// Distribute grenades to other users connected to the lobby.
    /// </summary>
    /// <param name="grenadeAssignments">List of grenades to be distributed to connected users.</param>
    /// <returns>Task.</returns>
    Task DistributeGrenades(List<GrenadeAssignmentDto> grenadeAssignments);
}
