using Domain.Dto;
using Interface.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Inoperable documentation controller.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = false)]
public class SignalRClientDocumentationController : ControllerBase, ILobbyClient
{
    /// <summary>
    /// This method is invoked once after the client is connected.
    /// </summary>
    /// <param name="lobby">The current lobby.</param>
    /// <returns>Task.</returns>
    [HttpPatch("/" + nameof(ILobbyClient.Lobby))]
    public Task Lobby(ActiveLobbyDto lobby)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }

    /// <summary>
    /// This method is invoked when a new message is sent by a connected user.
    /// </summary>
    /// <param name="sender">Sender id.</param>
    /// <param name="message">Message.</param>
    /// <returns>Task.</returns>
    [HttpPatch("/" + nameof(ILobbyClient.MessageReceieved))]
    public Task MessageReceieved(Guid sender, string message)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }

    /// <summary>
    /// This method is invoked when the igl distributes grenades to connected users.
    /// </summary>
    /// <param name="grenadeAssignments">Grenades to be assigned.</param>
    /// <returns>Task.</returns>
    [HttpPatch("/" + nameof(ILobbyClient.GrenadeAssignmentsReceived))]
    public Task GrenadeAssignmentsReceived(IEnumerable<GrenadeAssignmentDto> grenadeAssignments)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }

    /// <summary>
    /// This method is invoked when a user is joining, leaving or changing name.
    /// </summary>
    /// <param name="userInfo">Object with userid and changes.</param>
    /// <returns>Task.</returns>
    [HttpPatch("/" + nameof(ILobbyClient.UserInfo))]
    public Task UserInfo(UserInfoDto userInfo)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }
}
