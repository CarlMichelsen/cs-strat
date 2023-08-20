using Domain.Attribute;

/// <summary>
/// User model.
/// </summary>
[DtoDisplayName("User")]
public class UserDto
{
    /// <summary>
    /// Gets or initiates guid identifier for the user.
    /// </summary>
    /// <value>Guid Id.</value>
    required public Guid Id { get; init; }

    /// <summary>
    /// Gets or initiates name for the user.
    /// </summary>
    /// <value>User name.</value>
    required public string Name { get; init; }
}