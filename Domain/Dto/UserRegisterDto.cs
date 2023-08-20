namespace Domain.Dto;

/// <summary>
/// Register request model.
/// </summary>
public class UserRegisterDto
{
    /// <summary>
    /// Gets or initiates name for the user.
    /// </summary>
    /// <value>User name.</value>
    required public string Name { get; init; }
}