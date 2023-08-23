using Domain.Dto;
using Domain.Entity;
using Domain.Lobby;

namespace BusinessLogic.Mapper;

/// <summary>
/// Mapper for active lobby.
/// </summary>
public static class ActiveLobbyMapper
{
    /// <summary>
    /// Maps an ActiveLobby to an ActiveLobbyDto.
    /// </summary>
    /// <param name="activeLobby">ActiveLobby.</param>
    /// <returns>ActiveLobbyDto.</returns>
    public static ActiveLobbyDto Map(ActiveLobby activeLobby)
    {
        return new ActiveLobbyDto
        {
        };
    }

    /// <summary>
    /// Maps an ActiveLobby to an ActiveLobbyDto.
    /// </summary>
    /// <param name="lobbyAccess">LobbyAccess entity.</param>
    /// <returns>ActiveLobby.</returns>
    public static ActiveLobby Map(LobbyAccess lobbyAccess)
    {
        return new ActiveLobby
        {
            Id = lobbyAccess.Id,
            UniqueHumanReadableIdentifier = lobbyAccess.UniqueHumanReadableIdentifier
                ?? throw new NullReferenceException("lobbyAccess.UniqueHumanReadableIdentifier is null during mapping."),
            Creator = lobbyAccess.Creator,
            InGameLeader = lobbyAccess.InGameLeader,
            Members = new Dictionary<Guid, MetaUser>(),
            CreatedTime = lobbyAccess.CreatedTime,
        };
    }
}
