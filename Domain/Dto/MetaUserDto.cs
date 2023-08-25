using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Wrapper for user that contains useful metadata about the connectionstate of the user.
/// </summary>
[DtoDisplayName("MetaUser")]
public class MetaUserDto
{
    /// <summary>
    /// Gets or sets online (connected) state of the user.
    /// </summary>
    /// <value>Boolean online value.</value>
    required public bool Online { get; set; }

    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <value>User.</value>
    required public UserDto User { get; init; }
}
