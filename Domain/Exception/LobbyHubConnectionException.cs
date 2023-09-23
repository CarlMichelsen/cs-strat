namespace Domain.Exception;

/// <summary>
/// Exception related to Cs-Strat repositories.
/// </summary>
public class LobbyHubConnectionException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHubConnectionException"/> class.
    /// </summary>
    public LobbyHubConnectionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHubConnectionException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public LobbyHubConnectionException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHubConnectionException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Inner Exception.</param>
    public LobbyHubConnectionException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}
