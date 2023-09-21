using Domain.Attribute;

/// <summary>
/// Info about changes to a user.
/// </summary>
[DtoDisplayName("UserInfo")]
public class UserInfoDto
{
    /// <summary>
    /// Gets id of the user with changes.
    /// </summary>
    required public Guid Id { get; init; }

    /// <summary>
    /// Gets whether the user is currently online.
    /// </summary>
    required public bool Online { get; init; }

    /// <summary>
    /// Gets the name of the user if it has changed. Null otherwise.
    /// </summary>
    /// <value>Nullable username, set if changed.</value>
    required public string? Name { get; init; }
}
