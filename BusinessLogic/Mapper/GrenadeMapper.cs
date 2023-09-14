using Domain.Dto;
using Domain.Lobby;

namespace BusinessLogic.Mapper;

/// <summary>
/// Mapper for grenades.
/// </summary>
public static class GrenadeMapper
{
    /// <summary>
    /// Map grenade to grenadeDto.
    /// </summary>
    /// <param name="grenade">Grenade to be mapped.</param>
    /// <returns>GrenadeDto.</returns>
    public static GrenadeDto Map(Grenade grenade)
    {
        return new GrenadeDto
        {
            Team = grenade.Team,
            Description = grenade.Description,
            ImageUrl = grenade.ImageUrl,
            Jumpthrow = grenade.Jumpthrow,
        };
    }

    /// <summary>
    /// Map grenadeDto to grenade.
    /// </summary>
    /// <param name="grenadeDto">grenadeDto to be mapped.</param>
    /// <returns>Grenade.</returns>
    public static Grenade Map(GrenadeDto grenadeDto)
    {
        return new Grenade
        {
            Team = grenadeDto.Team,
            Description = grenadeDto.Description,
            ImageUrl = grenadeDto.ImageUrl,
            Jumpthrow = grenadeDto.Jumpthrow,
        };
    }

    /// <summary>
    /// Map GrenadeAssignment to GrenadeAssignmentDto.
    /// </summary>
    /// <param name="grenadeAssignment">Object to be mapped.</param>
    /// <returns>Map-result object.</returns>
    public static GrenadeAssignmentDto Map(GrenadeAssignment grenadeAssignment)
    {
        return new GrenadeAssignmentDto
        {
            UserId = grenadeAssignment.UserId,
            Assignment = grenadeAssignment.Assignment is null ? null : Map(grenadeAssignment.Assignment),
        };
    }

    /// <summary>
    /// Map GrenadeAssignmentDto to GrenadeAssignment.
    /// </summary>
    /// <param name="grenadeAssignment">Object to be mapped.</param>
    /// <returns>Map-result object.</returns>
    public static GrenadeAssignment Map(GrenadeAssignmentDto grenadeAssignment)
    {
        return new GrenadeAssignment
        {
            UserId = grenadeAssignment.UserId,
            Assignment = grenadeAssignment.Assignment is null ? null : Map(grenadeAssignment.Assignment),
        };
    }
}
