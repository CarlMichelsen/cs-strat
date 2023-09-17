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
            Id = activeLobby.UniqueHumanReadableIdentifier,
            Creator = activeLobby.Creator,
            InGameLeader = activeLobby.InGameLeader,
            Members = activeLobby.Members.ToDictionary(
                pair => pair.Key,
                pair => Map(pair.Value)
            ),
            CreatedTime = activeLobby.CreatedTime,
        };
    }

    /// <summary>
    /// Map MetaUser to MetaUserDto.
    /// </summary>
    /// <param name="metaUser">MetaUser to map.</param>
    /// <returns>MetaUserDto.</returns>
    public static MetaUserDto Map(MetaUser metaUser)
    {
        return new MetaUserDto
        {
            Online = metaUser.Online,
            Name = metaUser.User.Name,
            GrenadeAssignment = metaUser.GrenadeAssignment is null
                ? default
                : GrenadeMapper.Map(metaUser.GrenadeAssignment),
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
            InGameLeader = lobbyAccess.Creator,
            Members = new Dictionary<Guid, MetaUser>(),
            CreatedTime = lobbyAccess.CreatedTime,
        };
    }
}
