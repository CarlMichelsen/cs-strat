using Domain.Entity;
using Interface.Repository;
using Interface.Service;

namespace BusinessLogic.Service;

/// <inheritdoc />
public class LobbyService : ILobbyService
{
    private readonly ILobbyAccessRepository lobbyRepository;
    private readonly IHumanReadableIdentifierService humanReadableIdentifierService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyService"/> class.
    /// </summary>
    /// <param name="lobbyRepository">Lobby repository.</param>
    /// <param name="humanReadableIdentifierService">Human readable identifier service.</param>
    public LobbyService(
        ILobbyAccessRepository lobbyRepository,
        IHumanReadableIdentifierService humanReadableIdentifierService)
    {
        this.lobbyRepository = lobbyRepository;
        this.humanReadableIdentifierService = humanReadableIdentifierService;
    }

    /// <inheritdoc />
    public Task<LobbyAccess> ClearUniqueHumanReadableIdentifier(string uniqueHumanReadableIdentifier)
    {
        return this.lobbyRepository.ClearUniqueHumanReadableIdentifier(uniqueHumanReadableIdentifier);
    }

    /// <inheritdoc />
    public async Task<LobbyAccess> CreateLobby(Guid creator)
    {
        var lobby = new LobbyAccess
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
    public Task<LobbyAccess> GetLobby(string uniqueHumanReadableIdentifier)
    {
        return this.lobbyRepository.GetLobby(uniqueHumanReadableIdentifier);
    }

    /// <inheritdoc />
    public Task<LobbyAccess> JoinLobby(string uniqueHumanReadableIdentifier, Guid user)
    {
        return this.lobbyRepository.JoinLobby(uniqueHumanReadableIdentifier, user);
    }
}
