using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Lobby model.
/// </summary>
[DtoDisplayName("LobbyData")]
public class LobbyDataDto
{
    /// <summary>
    /// Gets or sets unique human readable identifier used by users to join a lobby.
    /// If this identifier is null, the lobby is no longer joinable.
    /// </summary>
    /// <value>String unique human readable identifier.</value>
    required public string? UniqueHumanReadableIdentifier { get; set; }

    /// <summary>
    /// Gets or sets user that created the lobby.
    /// </summary>
    /// <value>Creator user.</value>
    required public Guid Creator { get; set; }

    /// <summary>
    /// Gets or sets current in game leader user.
    /// This should the creator by default.
    /// </summary>
    /// <value>In game leader user.</value>
    required public Guid InGameLeader { get; set; }

    /// <summary>
    /// Gets or sets list of all members of the lobby.
    /// </summary>
    /// <value>List of users that are members.</value>
    required public List<Guid> Members { get; set; }

    /// <summary>
    /// Gets or sets time the lobby was created.
    /// </summary>
    /// <value>Created time.</value>
    required public DateTime CreatedTime { get; set; }
}