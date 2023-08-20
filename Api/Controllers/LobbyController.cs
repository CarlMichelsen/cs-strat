using System.Security.Claims;
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
    public async Task<ActionResult<ServiceResponse<LobbyDataDto>>> CreateLobby()
    {
        Guid creatorGuid = this.GetIdentifierFromUser();

        var lobbyResponse = await this.lobbyHandler
            .CreateLobby(creatorGuid);

        if (!lobbyResponse.Ok)
        {
            return this.BadRequest(lobbyResponse);
        }

        return lobbyResponse;
    }

    /// <summary>
    /// Get an exsisting lobby.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Unique human readable lobby-identifier.</param>
    /// <returns>LobbyData.</returns>
    [HttpGet("{uniqueHumanReadableIdentifier}")]
    public async Task<ActionResult<ServiceResponse<LobbyDataDto>>> GetLobby(
        [FromRoute] string uniqueHumanReadableIdentifier)
    {
        var lobbyResponse = await this.lobbyHandler
            .GetLobby(uniqueHumanReadableIdentifier);

        if (!lobbyResponse.Ok)
        {
            return this.NotFound(lobbyResponse);
        }

        return lobbyResponse;
    }

    /// <summary>
    /// Join an exsisting lobby, the joiner is derived from cookie.
    /// </summary>
    /// <param name="uniqueHumanReadableIdentifier">Unique human readable lobby-identifier.</param>
    /// <returns>LobbyData.</returns>
    [HttpPut("{uniqueHumanReadableIdentifier}")]
    public async Task<ActionResult<ServiceResponse<LobbyDataDto>>> JoinLobby(
        [FromRoute] string uniqueHumanReadableIdentifier)
    {
        Guid joinerGuid = this.GetIdentifierFromUser();

        var lobbyResponse = await this.lobbyHandler
            .JoinLobby(uniqueHumanReadableIdentifier, joinerGuid);

        if (!lobbyResponse.Ok)
        {
            return this.NotFound(lobbyResponse);
        }

        return lobbyResponse;
    }

    private Guid GetIdentifierFromUser()
    {
        var claimValue = this.HttpContext.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(claimValue);
    }
}