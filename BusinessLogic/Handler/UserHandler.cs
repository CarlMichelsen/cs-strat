using System.Security.Claims;
using BusinessLogic.Mapper;
using Domain.Configuration;
using Domain.Dto;
using Interface.Handler;
using Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessLogic.Handler;

/// <inheritdoc />
public class UserHandler : BaseHandler, IUserHandler
{
    private readonly ILogger<UserHandler> logger;
    private readonly IJwtService jwtService;
    private readonly IOptions<JwtOptions> jwtOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger for.. logging...</param>
    /// <param name="jwtService">Service for generating jwt tokens.</param>
    /// <param name="jwtOptions">configuration for cookie writing.</param>
    public UserHandler(
        ILogger<UserHandler> logger,
        IJwtService jwtService,
        IOptions<JwtOptions> jwtOptions)
    {
        this.logger = logger;
        this.jwtService = jwtService;
        this.jwtOptions = jwtOptions;
    }

    /// <inheritdoc />
    public ServiceResponse<UserDto> Register(UserRegisterDto user, ClaimsPrincipal claimsPrincipal, IResponseCookies cookies)
    {
        return this.MapToServiceResponseSync<UserDto>(this.logger, () =>
        {
            var domainUser = UserMapper.MapWithExsistingOrNewGuid(user, claimsPrincipal);

            var token = this.jwtService.GenerateJwtToken(domainUser);

            // Add cookie.
            cookies.Append(
                this.jwtOptions.Value.CookieName,
                token,
                this.GetCookieOptions());

            return UserMapper.Map(domainUser);
        });
    }

    /// <inheritdoc />
    public ServiceResponse<UserDto> WhoAmI(ClaimsPrincipal claimsPrincipal)
    {
        return this.MapToServiceResponseSync<UserDto>(this.logger, () => UserMapper.Map(UserMapper.Map(claimsPrincipal)));
    }

    private CookieOptions GetCookieOptions()
    {
        return new CookieOptions
        {
            // Set the secure flag, which Chrome's SameSite rules require for SameSite=None
            Secure = true,

            // Set the cookie to HTTP only, which is good for security.
            HttpOnly = true,

            // Set the SameSite mode for the cookie.
            SameSite = SameSiteMode.None,

            // Expire time for the cookie.
            Expires = DateTime.Now.AddDays(this.jwtOptions.Value.TTLDays),
        };
    }
}
