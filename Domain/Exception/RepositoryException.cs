namespace Domain.Exception;

/// <summary>
/// Exception related to Cs-Strat repositories.
/// </summary>
public class RepositoryException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    public RepositoryException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public RepositoryException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="inner">Inner Exception.</param>
    public RepositoryException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}