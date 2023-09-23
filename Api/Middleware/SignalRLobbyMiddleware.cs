using Interface.Factory;
using Domain.Configuration;

namespace Api.Middleware;

/// <summary>
/// SignalR lobby member Authorization.
/// This should stop people who are not a member in a lobby from joining it.
/// </summary>
public class SignalRLobbyMiddleware : IMiddleware
{
    private readonly ILogger<SignalRLobbyMiddleware> logger;
    private readonly ILobbyAuthServiceFactory lobbyAuthServiceFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CookieMiddleware"/> class.
    /// </summary>
    /// <param name="logger">Errorlog.</param>
    /// <param name="lobbyAuthServiceFactory">Factory for authorization dependencies in case they are needed.</param>
    public SignalRLobbyMiddleware(
        ILogger<SignalRLobbyMiddleware> logger,
        ILobbyAuthServiceFactory lobbyAuthServiceFactory)
    {
        this.logger = logger;
        this.lobbyAuthServiceFactory = lobbyAuthServiceFactory;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Path.StartsWithSegments(ApplicationConstants.LobbySignalREndpoint))
        {
            await next(context);
            return;
        }

        var userConnectionContext = await this.HandleConnection(context);

        if (userConnectionContext is not null)
        {
            context.Items.Add(ApplicationConstants.LobbyUserConnectionContext, userConnectionContext);
            await next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Not allowed");
    }

    private async Task<Domain.Lobby.UserConnectionContext?> HandleConnection(HttpContext context)
    {
        try
        {
            var lobbyAuthService = this.lobbyAuthServiceFactory.Create();
            return await lobbyAuthService.Connect(context.User, context.Request.Query);
        }
        catch (System.Exception e)
        {
            this.logger.LogCritical("Error during SignalRLobbyMiddleware authorization: {}", e);
            return default;
        }
    }
}
