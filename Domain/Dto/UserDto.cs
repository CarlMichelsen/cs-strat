using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// User.
/// </summary>
[DtoDisplayName("UserRecord")]
public class UserDto
{
    /// <summary>
    /// Gets user identifier.
    /// </summary>
    /// <value>Guid identifier.</value>
    required public Guid Id { get; init; }

    /// <summary>
    /// Gets user name.
    /// </summary>
    /// <value>String name of user.</value>
    required public string Name { get; init; }
}
