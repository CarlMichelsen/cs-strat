namespace Domain.Configuration;

/// <summary>
/// Application-wide compile time constants.
/// </summary>
public static class ApplicationConstants
{
    /// <summary>
    /// Name of the currently only role assigned in jwt tokens.
    /// </summary>
    public const string UserRole = "user";

    /// <summary>
    /// Endpoint for signalR lobby connections.
    /// </summary>
    public const string LobbySignalREndpoint = "/LobbyHub";

    /// <summary>
    /// Name of frontend CORS policy.
    /// </summary>
    public const string FrontendCorsPolicy = "FrontendAllowed";

    /// <summary>
    /// Name of connectioncontext in signalR Context.Items and handshake Context.Items.
    /// </summary>
    public const string LobbyUserConnectionContext = "UserConnectionContext";

    /// <summary>
    /// Name of expected query parameter during a signalR lobby connection attempt.
    /// </summary>
    public const string LobbyIdQueryName = "lobbyId";
}
