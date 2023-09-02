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
            Name = grenade.Name,
            ImageUrl = grenade.ImageUrl,
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
            Name = grenadeDto.Name,
            ImageUrl = grenadeDto.ImageUrl,
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
            User = grenadeAssignment.User,
            Assignment = Map(grenadeAssignment.Assignment),
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
            User = grenadeAssignment.User,
            Assignment = Map(grenadeAssignment.Assignment),
        };
    }
}
