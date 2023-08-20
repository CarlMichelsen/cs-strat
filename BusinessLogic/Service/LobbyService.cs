using Domain.Entity;
using Interface.Repository;
using Interface.Service;

namespace Businesslogic.Service;

/// <inheritdoc />
public class LobbyService : ILobbyService
{
    private readonly ILobbyRepository lobbyRepository;
    private readonly IHumanReadableIdentifierService humanReadableIdentifierService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyService"/> class.
    /// </summary>
    /// <param name="lobbyRepository">Lobby repository.</param>
    /// <param name="humanReadableIdentifierService">Human readable identifier service.</param>
    public LobbyService(
        ILobbyRepository lobbyRepository,
        IHumanReadableIdentifierService humanReadableIdentifierService)
    {
        this.lobbyRepository = lobbyRepository;
        this.humanReadableIdentifierService = humanReadableIdentifierService;
    }

    /// <inheritdoc />
    public Task<Lobby> ClearUniqueHumanReadableIdentifier(string uniqueHumanReadableIdentifier)
    {
        return this.lobbyRepository.ClearUniqueHumanReadableIdentifier(uniqueHumanReadableIdentifier);
    }

    /// <inheritdoc />
    public async Task<Lobby> CreateLobby(Guid creator)
    {
        var lobby = new Lobby
        {
            UniqueHumanReadableIdentifier = await this.humanReadableIdentifierService.GenerateUniqueIdentifier(),
            Creator = creator,
            InGameLeader = creator,
            Members = new List<Guid> { creator },
            CreatedTime = DateTime.UtcNow,
        };

        return await this.lobbyRepository.CreateLobby(lobby);
    }

    /// <inheritdoc />
    public Task<Lobby> GetLobby(string uniqueHumanReadableIdentifier)
    {
        return this.lobbyRepository.GetLobby(uniqueHumanReadableIdentifier);
    }

    /// <inheritdoc />
    public Task<Lobby> JoinLobby(string uniqueHumanReadableIdentifier, Guid user)
    {
        return this.lobbyRepository.JoinLobby(uniqueHumanReadableIdentifier, user);
    }
}