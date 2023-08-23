using Api.Extension;
using Domain.Dto;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller for lobby manipulation.
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "user")]
public class LobbyController : ControllerBase
{
    private readonly ILobbyHandler lobbyHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyController"/> class.
    /// </summary>
    /// <param name="lobbyHandler">Handler for lobbies.</param>
    public LobbyController(
        ILobbyHandler lobbyHandler)
    {
        this.lobbyHandler = lobbyHandler;
    }

    /// <summary>
    /// Create a lobby, the creator of the lobby is derived from cookie.
    /// </summary>
    /// <returns>LobbyData.</returns>
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<string>>> CreateLobby()
    {
        var creator = this.HttpContext.User.GetUserModel();

        var lobbyResponse = await this.lobbyHandler
            .CreateLobby(creator.Id);

        return lobbyResponse;
    }

    /// <summary>
    /// Join an exsisting lobby, the joiner is derived from cookie.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Unique human readable lobby-identifier.</param>
    /// <returns>LobbyData.</returns>
    [HttpPut("{uniqueHumanReadableIdentifier}")]
    public async Task<ActionResult<ServiceResponse<string>>> JoinLobby(
        [FromRoute] string uniqueHumanReadableIdentifier)
    {
        var joiner = this.HttpContext.User.GetUserModel();

        var lobbyResponse = await this.lobbyHandler
            .JoinLobby(uniqueHumanReadableIdentifier, joiner.Id);

        return lobbyResponse;
    }
}
