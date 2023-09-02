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
public class SignalRServerDocumentationController : ControllerBase, ILobbyServer
{
    /// <summary>
    /// Invoke this method to send a message to all users connected to your lobby.
    /// </summary>
    /// <param name="message">The content of the message.</param>
    /// <returns>Task.</returns>
    [HttpPatch("/" + nameof(ILobbyServer.Message))]
    public Task Message(string message)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }

    /// <summary>
    /// Invoke this method to send a list of grenades to be randomly distributed to connected lobby users.
    /// </summary>
    /// <param name="grenadeAssignments">List of grenades to distribute.</param>
    /// <returns>Task.</returns>
    [HttpPatch("/" + nameof(ILobbyServer.DistributeGrenades))]
    public Task DistributeGrenades(List<GrenadeAssignmentDto> grenadeAssignments)
    {
        throw new NotImplementedException("This method is for documentation purposes.");
    }
}
