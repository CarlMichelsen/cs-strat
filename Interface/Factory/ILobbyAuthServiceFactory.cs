using Interface.Service;

namespace Interface.Factory;

/// <summary>
/// A factory that can create the <see cref="ILobbyAuthService"/>.
/// </summary>
public interface ILobbyAuthServiceFactory
{
    /// <summary>
    /// Create an instance of <see cref="ILobbyAuthService"/> from dependency injection.
    /// </summary>
    ILobbyAuthService Create();
}
