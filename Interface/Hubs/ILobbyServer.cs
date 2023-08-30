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
    /// Send a message that will be echoed to everyone in the lobby.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <returns>Task.</returns>
    Task Message(string message);

    /// <summary>
    /// Distribute grenades to other users connected to the lobby.
    /// </summary>
    /// <param name="grenades">List of grenades to distribute.</param>
    /// <returns>Task.</returns>
    Task DistributeGrenades(List<GrenadeDto> grenades);
}
