using Domain.Lobby;

namespace Interface.Service;

/// <summary>
/// Service for generating jwt tokens for users.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generate a jwt token for a user.
    /// </summary>
    /// <param name="user">User to generate token for.</param>
    /// <returns>JWT token for user.</returns>
    string GenerateJwtToken(User user);
}