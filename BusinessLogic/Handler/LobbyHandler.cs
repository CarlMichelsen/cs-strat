using Businesslogic.Mapper;
using Domain.Dto;
using Domain.Exception;
using Interface.Handler;
using Interface.Service;
using Microsoft.Extensions.Logging;

namespace Businesslogic.Handler;

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
    public Task<ServiceResponse<LobbyDataDto>> CreateLobby(Guid creator)
    {
        return this.MapToServiceResponse<LobbyDataDto>(this.logger, async () =>
        {
            var lobby = await this.lobbyService.CreateLobby(creator);
            return LobbyDataDtoMapper.Map(lobby);
        });
    }

    /// <inheritdoc />
    public Task<ServiceResponse<LobbyDataDto>> GetLobby(string uniqueHumanReadableIdentifier)
    {
        return this.MapToServiceResponse<LobbyDataDto>(this.logger, async () =>
        {
            var lobby = await this.lobbyService.GetLobby(uniqueHumanReadableIdentifier);
            return LobbyDataDtoMapper.Map(lobby);
        });
    }

    /// <inheritdoc />
    public Task<ServiceResponse<LobbyDataDto>> JoinLobby(string uniqueHumanReadableIdentifier, Guid user)
    {
        return this.MapToServiceResponse<LobbyDataDto>(this.logger, async () =>
        {
            var lobby = await this.lobbyService.JoinLobby(uniqueHumanReadableIdentifier, user);
            return LobbyDataDtoMapper.Map(lobby);
        });
    }
}