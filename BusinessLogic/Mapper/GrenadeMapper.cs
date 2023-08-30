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
            GrenadeType = nameof(grenade.GrenadeType),
            HitLocation = grenade.HitLocation,
            StartLocation = grenade.StartLocation,
            Method = nameof(grenade.Method),
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
            GrenadeType = Enum.Parse<GrenadeType>(grenadeDto.GrenadeType),
            HitLocation = grenadeDto.HitLocation,
            StartLocation = grenadeDto.StartLocation,
            Method = Enum.Parse<ThrowMethod>(grenadeDto.Method),
            ImageUrl = grenadeDto.ImageUrl,
        };
    }
}
