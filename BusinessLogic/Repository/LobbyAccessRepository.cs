using BusinessLogic.Database;
using Domain.Entity;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository;

/// <inheritdoc />
public class LobbyAccessRepository : ILobbyAccessRepository
{
    private const string NotFoundExceptionText = "Did not find a lobby from the given uniqueHumanReadableIdentifier.";

    private readonly ApplicationContext applicationContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyAccessRepository"/> class.
    /// </summary>
    /// <param name="applicationContext">Application data context.</param>
    public LobbyAccessRepository(
        ApplicationContext applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    /// <inheritdoc />
    public async Task<LobbyAccess?> ClearUniqueHumanReadableIdentifier(string uniqueHumanReadableIdentifier)
    {
        var lobby = await this.applicationContext.Lobby
            .Where(l => l.UniqueHumanReadableIdentifier == uniqueHumanReadableIdentifier)
            .FirstOrDefaultAsync();

        if (lobby is not null)
        {
            lobby.UniqueHumanReadableIdentifier = null;
            await this.applicationContext.SaveChangesAsync();
        }

        return lobby;
    }

    /// <inheritdoc />
    public async Task<LobbyAccess> CreateLobby(LobbyAccess lobby)
    {
        this.applicationContext.Lobby.Add(lobby);

        await this.applicationContext.SaveChangesAsync();

        return lobby;
    }

    /// <inheritdoc />
    public async Task<LobbyAccess?> GetLobby(string uniqueHumanReadableIdentifier)
    {
        return await this.applicationContext.Lobby
            .FirstOrDefaultAsync(l => l.UniqueHumanReadableIdentifier == uniqueHumanReadableIdentifier);
    }

    /// <inheritdoc />
    public async Task<LobbyAccess?> JoinLobby(string uniqueHumanReadableIdentifier, Guid user)
    {
        var lobby = await this.GetLobby(uniqueHumanReadableIdentifier);

        if (lobby is not null && !lobby.Members.Any(u => u == user))
        {
            lobby.Members.Add(user);
            await this.applicationContext.SaveChangesAsync();
        }

        return lobby;
    }
}
