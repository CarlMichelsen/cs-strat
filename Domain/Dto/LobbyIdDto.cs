using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Model that contains a lobby-id that can be connected to.
/// </summary>
[DtoDisplayName("LobbyId")]
public class LobbyIdDto
{
    /// <summary>
    /// Gets human readable identifier for a lobby that can be joined.
    /// </summary>
    /// <value>String identifier.</value>
    required public string LobbyId { get; init; }
}
