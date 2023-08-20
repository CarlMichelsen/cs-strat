using Domain.Dto;
using Domain.Entity;

namespace Businesslogic.Mapper;

/// <summary>
/// Mapper for Lobby and LobbyDto.
/// </summary>
public static class LobbyDataDtoMapper
{
    /// <summary>
    /// Maps Lobby to LobbyDataDto.
    /// </summary>
    /// <param name="lobby">Lobby database entity.</param>
    /// <returns>LobbyDataDto.</returns>
    public static LobbyDataDto Map(Lobby lobby)
    {
        return new LobbyDataDto
        {
            UniqueHumanReadableIdentifier = lobby.UniqueHumanReadableIdentifier,
            Creator = lobby.Creator,
            InGameLeader = lobby.InGameLeader,
            Members = lobby.Members.ToList(),
            CreatedTime = lobby.CreatedTime,
        };
    }
}