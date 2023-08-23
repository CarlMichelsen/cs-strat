using BusinessLogic.Mapper;
using Domain.Dto;
using Interface.Handler;
using Interface.Service;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Handler;

/// <inheritdoc />
public class LobbyHandler : BaseHandler, ILobbyHandler
{
    private readonly ILogger<LobbyHandler> logger;
    private readonly ILobbyService lobbyService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger for.. logging...</param>
    /// <param name="lobbyService">Lobby service for manipulating lobbies.</param>
    public LobbyHandler(
        ILogger<LobbyHandler> logger,
        ILobbyService lobbyService)
    {
        this.logger = logger;
        this.lobbyService = lobbyService;
    }

    /// <inheritdoc />
    public Task<ServiceResponse<string>> CreateLobby(Guid creator)
    {
        return this.MapToServiceResponse<string>(this.logger, async () =>
        {
            var lobby = await this.lobbyService.CreateLobby(creator);
            if (lobby.UniqueHumanReadableIdentifier is null)
            {
                throw new NullReferenceException("UniqueHumanReadableIdentifier null");
            }

            return lobby.UniqueHumanReadableIdentifier;
        });
    }

    /// <inheritdoc />
    public Task<ServiceResponse<string>> JoinLobby(string uniqueHumanReadableIdentifier, Guid user)
    {
        return this.MapToServiceResponse<string>(this.logger, async () =>
        {
            var lobby = await this.lobbyService.JoinLobby(uniqueHumanReadableIdentifier, user);
            if (lobby.UniqueHumanReadableIdentifier is null)
            {
                throw new NullReferenceException("UniqueHumanReadableIdentifier null");
            }

            return lobby.UniqueHumanReadableIdentifier;
        });
    }
}
