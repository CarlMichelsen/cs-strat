namespace Domain.Exception;

/// <summary>
/// Exception related to Cs-Strat lobby auth.
/// </summary>
public class LobbyAuthException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyAuthException"/> class.
    /// </summary>
    public LobbyAuthException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyAuthException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public LobbyAuthException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyAuthException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Inner Exception.</param>
    public LobbyAuthException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}
