using Domain.Lobby;
using Interface.Hubs;

namespace Interface.Handler;

/// <summary>
/// Handler for lobbyHub.
/// </summary>
public interface ILobbyHubHandler: ILobbyServer
{
    /// <summary>
    /// Get or set method for getting usercontext.
    /// </summary>
    Func<UserConnectionContext?> UserContext { get; set; }

    /// <summary>
    /// Interface to invoke client methods.
    /// </summary>
    ILobbyClient? Group { get; set; }

    /// <summary>
    /// Handle disconnection.
    /// </summary>
    /// <param name="exception">If the disconnect happened because of an exception.</param>
    Task OnDisconnectedAsync(Exception? exception);
}
