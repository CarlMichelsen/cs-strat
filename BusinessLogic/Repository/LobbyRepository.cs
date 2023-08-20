using Businesslogic.Database;
using Domain.Entity;
using Domain.Exception;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Businesslogic.Repository;

/// <inheritdoc />
public class LobbyRepository : ILobbyRepository
{
    private const string NotFoundExceptionText = "Did not find a lobby from the given uniqueHumanReadableIdentifier.";

    private readonly ApplicationContext applicationContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LobbyRepository"/> class.
    /// </summary>
    /// <param name="applicationContext">Application data context.</param>
    public LobbyRepository(
        ApplicationContext applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    /// <inheritdoc />
    public async Task<Lobby> ClearUniqueHumanReadableIdentifier(string uniqueHumanReadableIdentifier)
    {
        var lobby = await this.applicationContext.Lobby
            .Where(l => l.UniqueHumanReadableIdentifier == uniqueHumanReadableIdentifier)
            .FirstOrDefaultAsync();

        if (lobby is null)
        {
            throw new RepositoryException(NotFoundExceptionText);
        }

        lobby.UniqueHumanReadableIdentifier = null;

        await this.applicationContext.SaveChangesAsync();

        return lobby;
    }

    /// <inheritdoc />
    public async Task<Lobby> CreateLobby(Lobby lobby)
    {
        this.applicationContext.Lobby.Add(lobby);

        await this.applicationContext.SaveChangesAsync();

        return lobby;
    }

    /// <inheritdoc />
    public async Task<Lobby> GetLobby(string uniqueHumanReadableIdentifier)
    {
        return await this.applicationContext.Lobby
            .FirstOrDefaultAsync(l => l.UniqueHumanReadableIdentifier == uniqueHumanReadableIdentifier)
                ?? throw new RepositoryException(NotFoundExceptionText);
    }

    /// <inheritdoc />
    public async Task<Lobby> JoinLobby(string uniqueHumanReadableIdentifier, Guid user)
    {
        var lobby = await this.GetLobby(uniqueHumanReadableIdentifier);

        if (!lobby.Members.Any(u => u == user))
        {
            lobby.Members.Add(user);
            await this.applicationContext.SaveChangesAsync();
        }
        else
        {
            throw new RepositoryException("User is already in the lobby");
        }

        return lobby;
    }
}