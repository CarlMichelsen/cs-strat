using Domain.Configuration;
using Microsoft.Extensions.Options;

namespace Api.Middleware;

/// <summary>
/// Middleware that moves cookie into Authorization header to allow Bearer Authorization.
/// </summary>
public class CookieMiddleware : IMiddleware
{
    private readonly IOptions<JwtOptions> jwtOptions;
    private readonly ILogger<CookieMiddleware> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CookieMiddleware"/> class.
    /// </summary>
    /// <param name="jwtOptions">Configuration for jwt.</param>
    /// <param name="logger">A general logger interface.</param>
    public CookieMiddleware(
        IOptions<JwtOptions> jwtOptions,
        ILogger<CookieMiddleware> logger)
    {
        this.jwtOptions = jwtOptions;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var jwt = context.Request.Cookies[this.jwtOptions.Value.CookieName];

        if (!string.IsNullOrWhiteSpace(jwt))
        {
            var authorizationHeader = $"Bearer {jwt}";
            context.Request.Headers.TryAdd("Authorization", authorizationHeader);

            this.logger.LogDebug("{} cookie was found and reassigned to Authorization Bearer", this.jwtOptions.Value.CookieName);
        }
        else
        {
            this.logger.LogDebug("{} cookie was NOT found and reassigned to Authorization Bearer", this.jwtOptions.Value.CookieName);
        }

        await next(context);
    }
}