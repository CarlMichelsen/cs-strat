using Interface.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

/// <summary>
/// SignalR hub for real-time communication.
/// </summary>
[Authorize(Roles = "user")]
public class LobbyHub : Hub<ILobbyClient>
{
}