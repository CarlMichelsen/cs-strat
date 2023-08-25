using Domain.Entity;
using Interface.Repository;

namespace BusinessLogic.Repository;

/// <inheritdoc />
public class LobbyAccessMemoryRepository : ILobbyAccessRepository
{
    private readonly static Dictionary<string, LobbyAccess> memory = new();

    /// <inheritdoc />
    public Task<LobbyAccess?> ClearUniqueHumanReadableIdentifier(string uniqueHumanReadableIdentifier)
    {
        if (memory.TryGetValue(uniqueHumanReadableIdentifier, out var value))
        {
            value.UniqueHumanReadableIdentifier = default;
            return Task.FromResult<LobbyAccess?>(value);
        }

        return Task.FromResult<LobbyAccess?>(default);
    }

    /// <inheritdoc />
    public Task<LobbyAccess> CreateLobby(LobbyAccess lobby)
    {
        memory.Add(
            lobby.UniqueHumanReadableIdentifier ?? throw new NullReferenceException("No UniqueHumanReadableIdentifier"),
            lobby);

        return Task.FromResult(lobby);
    }

    /// <inheritdoc />
    public Task<LobbyAccess?> GetLobby(string uniqueHumanReadableIdentifier)
    {
        if (memory.TryGetValue(uniqueHumanReadableIdentifier, out var value))
        {
            return Task.FromResult<LobbyAccess?>(value);
        }

        return Task.FromResult<LobbyAccess?>(default);
    }

    /// <inheritdoc />
    public Task<LobbyAccess?> JoinLobby(string uniqueHumanReadableIdentifier, Guid user)
    {
        if (memory.TryGetValue(uniqueHumanReadableIdentifier, out var value))
        {
            if (!value.Members.Any(u => u == user))
            {
                value.Members.Add(user);
            }

            return Task.FromResult<LobbyAccess?>(value);
        }

        return Task.FromResult<LobbyAccess?>(default);
    }
}
