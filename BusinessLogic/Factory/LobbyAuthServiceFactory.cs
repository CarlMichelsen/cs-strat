using Interface.Factory;
using Interface.Service;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic.Factory;

/// <inheritdoc />
public class LobbyAuthServiceFactory : ILobbyAuthServiceFactory
{
    private readonly IServiceProvider services;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyAuthServiceFactory"/> class.
    /// </summary>
    public LobbyAuthServiceFactory(
        IServiceProvider services)
    {
        this.services = services;
    }

    /// <inheritdoc />
    public ILobbyAuthService Create()
    {
        return this.services.GetRequiredService<ILobbyAuthService>()
            ?? throw new NullReferenceException("Unable to create an instance of ILobbyAuthService from factory with dependency injection.");
    }
}
