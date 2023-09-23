using Domain.Dto;
using Domain.Configuration;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller for lobby manipulation.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserHandler userHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userHandler">Handler for users.</param>
    public UserController(
        IUserHandler userHandler)
    {
        this.userHandler = userHandler;
    }

    /// <summary>
    /// Register a user with a name.
    /// The response will contain a Set-Cookie header with a jwt token.
    /// If the user calls this endpoint with a valid cookie already, the identifier will be re-used so its only a name-change.
    /// Namechanges should not break anything.
    /// </summary>
    /// <param name="user">Userdata.</param>
    /// <returns>Userdata and a Set-Cookie header.</returns>
    [HttpPost]
    public ActionResult<ServiceResponse<UserDto>> Register([FromBody] UserRegisterDto user)
    {
        var lobbyResponse = this.userHandler.Register(user, this.HttpContext.User, this.Response.Cookies);

        return this.Ok(lobbyResponse);
    }

    /// <summary>
    /// Get the information contained in your http-only cookie.
    /// </summary>
    /// <returns>Userdata.</returns>
    [Authorize(Roles = ApplicationConstants.UserRole)]
    [HttpGet]
    public ActionResult<ServiceResponse<UserDto>> WhoAmI()
    {
        var lobbyResponse = this.userHandler.WhoAmI(this.HttpContext.User);

        return this.Ok(lobbyResponse);
    }
}
