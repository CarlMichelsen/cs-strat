using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Configuration;
using Domain.Lobby;
using Interface.Service;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogic.Service;

/// <inheritdoc />
public class JwtService : IJwtService
{
    private readonly IOptions<JwtOptions> jwtOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="jwtOptions">Options for jwt generation.</param>
    public JwtService(
        IOptions<JwtOptions> jwtOptions)
    {
        this.jwtOptions = jwtOptions;
    }

    /// <inheritdoc />
    public string GenerateJwtToken(User user)
    {
        var sub = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, ApplicationConstants.UserRole),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Role, ApplicationConstants.UserRole),
        });

        return this.GenerateToken(sub);
    }

    private string GenerateToken(ClaimsIdentity subject)
    {
        var key = Encoding.ASCII.GetBytes(this.jwtOptions.Value.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Issuer = this.jwtOptions.Value.Issuer,
            Audience = this.jwtOptions.Value.Audience,
            Expires = DateTime.UtcNow.AddDays(this.jwtOptions.Value.TTLDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
