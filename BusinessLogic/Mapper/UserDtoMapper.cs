using System.Security.Claims;
using Domain.Dto;
using Domain.Lobby;

namespace Businesslogic.Mapper;

/// <summary>
/// Mapper for users.
/// </summary>
public static class UserDtoMapper
{
    /// <summary>
    /// Maps a userDto to a user with a new random Guid.
    /// </summary>
    /// <param name="userDto">UserDto to map from.</param>
    /// <param name="claimsPrincipal">Claims to determine if user is already authenticated, then re-use the id.</param>
    /// <returns>Domain User.</returns>
    public static User MapWithExsistingOrNewGuid(
        UserRegisterDto userDto,
        ClaimsPrincipal claimsPrincipal)
    {
        var idClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        return new User
        {
            Id = idClaim is null ? Guid.NewGuid() : Guid.Parse(idClaim.Value),
            Name = userDto.Name,
        };
    }

    /// <summary>
    /// Map ClaimsPrincipal to user.
    /// </summary>
    /// <param name="claimsPrincipal">ClaimsPrincipal to map from.</param>
    /// <returns>User.</returns>
    public static User Map(ClaimsPrincipal claimsPrincipal)
    {
        var nameIdentifierClaim = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        var givenNameClaim = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.GivenName);

        return new User
        {
            Id = Guid.Parse(nameIdentifierClaim.Value),
            Name = givenNameClaim.Value,
        };
    }

    /// <summary>
    /// Map user to userDto.
    /// </summary>
    /// <param name="user">Domain user.</param>
    /// <returns>UserDto.</returns>
    public static UserDto Map(
        User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
        };
    }
}
