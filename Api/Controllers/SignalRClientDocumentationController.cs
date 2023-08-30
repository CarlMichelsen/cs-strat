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
    [HttpPatch(nameof(ILobbyClient.Lobby))]
    public Task Lobby(ActiveLobbyDto lobby)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }

    /// <summary>
    /// This method is invoked when a new message is sent by a connected user.
    /// </summary>
    /// <param name="user">Sender.</param>
    /// <param name="message">Message.</param>
    /// <returns>Task.</returns>
    [HttpPatch(nameof(ILobbyClient.MessageReceieved))]
    public Task MessageReceieved(UserDto user, string message)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }

    /// <summary>
    /// This method is invoked when the igl distributes grenades to connected users.
    /// </summary>
    /// <param name="grenade">Grenade to throw.</param>
    /// <returns>Task.</returns>
    [HttpPatch(nameof(ILobbyClient.GrenadeReceived))]
    public Task GrenadeReceived(GrenadeDto grenade)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }

    /// <summary>
    /// This method is invoked when a user is joining, leaving or changing name.
    /// </summary>
    /// <param name="metaUser">The relevant user wrapped in a metadata object.</param>
    /// <returns>Task.</returns>
    [HttpPatch(nameof(ILobbyClient.User))]
    public new Task User(MetaUserDto metaUser)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }
}
