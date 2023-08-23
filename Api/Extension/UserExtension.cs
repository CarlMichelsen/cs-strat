using Domain.Lobby;
using BusinessLogic.Mapper;

namespace Api.Extension;

/// <summary>
/// Extension method to get data from claimsPrincipal.
/// </summary>
public static class UserExtension
{
    /// <summary>
    /// Get User model from claimsPrincipal.
    /// </summary>
    /// <param name="claimsPrincipal">ClaimsPrincipal to get user from.</param>
    /// <returns>User model.</returns>
    public static User GetUserModel(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
    {
        return UserMapper.Map(claimsPrincipal);
    }
}
