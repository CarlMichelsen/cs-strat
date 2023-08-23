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
}
