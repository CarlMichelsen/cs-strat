using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Grenade assignment.
/// </summary>
[DtoDisplayName("GrenadeAssignment")]
public class GrenadeAssignmentDto
{
    /// <summary>
    /// Gets the user that should throw this grenade.
    /// </summary>
    /// <value>UserId.</value>
    required public Guid User { get; init; }

    /// <summary>
    /// Gets the grenade to be assigned.
    /// </summary>
    /// <value>Grenade.</value>
    required public GrenadeDto Assignment { get; init; }
}
