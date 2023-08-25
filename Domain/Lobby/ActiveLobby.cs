namespace Domain.Lobby;

/// <summary>
/// Model that represents a lobby inside a signalR hub.
/// The data in this lobby is volatile and will be lost when leaving memory.
/// </summary>
public class ActiveLobby
{
    /// <summary>
    /// Gets or sets database primary key identifier for activelobby.
    /// </summary>
    /// <value>Integer primary key value.</value>
    required public int Id { get; set; }

    /// <summary>
    /// Gets or sets unique human readable identifier used by users to join a lobby.
    /// </summary>
    /// <value>String unique human readable identifier.</value>
    required public string UniqueHumanReadableIdentifier { get; set; }

    /// <summary>
    /// Gets or sets id of user that created the lobby.
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
    /// Dictionary of users currently connected to the
    /// </summary>
    /// <value></value>
    required public Dictionary<Guid, MetaUser> Members { get; init; }

    /// <summary>
    /// Gets or sets time the lobby was created.
    /// </summary>
    /// <value>Created time.</value>
    required public DateTime CreatedTime { get; set; }
}
