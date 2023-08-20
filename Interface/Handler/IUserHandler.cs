using Domain.Dto;
using Microsoft.AspNetCore.Http;

namespace Interface.Handler;

/// <summary>
/// Handler for getting and creating users.
/// </summary>
public interface IUserHandler
{
    /// <summary>
    /// Registers the user by creating a jwt with user information in it, then writing that jwt to client cookie.
    /// </summary>
    /// <param name="user">User information.</param>
    /// <param name="claimsPrincipal">Claims to check for exsisting jwt token.</param>
    /// <param name="cookies">IResponseCookies interface to set cookie.</param>
    /// <returns>UserDto.</returns>
    ServiceResponse<UserDto> Register(UserRegisterDto user, System.Security.Claims.ClaimsPrincipal claimsPrincipal, IResponseCookies cookies);

    /// <summary>
    /// Returns data about the user fetched from the http-only cookie (that are moved to Authentication Bearer header in middleware).
    /// </summary>
    /// <param name="claimsPrincipal">Claims to read user info from.</param>
    /// <returns>UserDto.</returns>
    ServiceResponse<UserDto> WhoAmI(System.Security.Claims.ClaimsPrincipal claimsPrincipal);
}