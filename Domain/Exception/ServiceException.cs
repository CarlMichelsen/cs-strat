namespace Domain.Exception;

/// <summary>
/// Exception related to Cs-Strat repositories.
/// </summary>
public class ServiceException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceException"/> class.
    /// </summary>
    public ServiceException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public ServiceException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Inner Exception.</param>
    public ServiceException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}