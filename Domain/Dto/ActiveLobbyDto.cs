using Domain.Attribute;

namespace Domain.Dto;

/// <summary>
/// Model that represents a lobby inside a signalR hub.
/// </summary>
[DtoDisplayName("Lobby")]
public class ActiveLobbyDto
{
    /// <summary>
    /// Gets or sets unique human readable identifier used by users to join a lobby.
    /// </summary>
    /// <value>String unique human readable identifier.</value>
    required public string Id { get; set; }

    /// <summary>
    /// Gets id of user that created the lobby.
    /// </summary>
    /// <value>Creator user.</value>
    required public Guid Creator { get; init; }

    /// <summary>
    /// Gets or sets current in game leader user id.
    /// This should the creator by default.
    /// </summary>
    /// <value>In game leader user.</value>
    required public Guid InGameLeader { get; set; }

    /// <summary>
    /// Gets Dictionary of users currently connected to the lobby.
    /// </summary>
    /// <value>Dictionary of metaUsers.</value>
    required public Dictionary<Guid, MetaUserDto> Members { get; init; }

    /// <summary>
    /// Gets or sets time the lobby was created.
    /// </summary>
    /// <value>Created time.</value>
    required public DateTime CreatedTime { get; set; }
}
