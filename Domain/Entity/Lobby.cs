using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;

/// <summary>
/// Lobby database entity.
/// </summary>
public class Lobby
{
    private const string Separator = ",";

    /// <summary>
    /// Gets or sets database primary key identifier for lobby entity.
    /// </summary>
    /// <value>Integer primary key value.</value>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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
    /// Gets members of the lobby in serialized format, as it would be stored in the database.
    /// </summary>
    /// <value>Member guids in a string.</value>
    public string SerializedMembers
    {
        get => string.Join(Separator, this.Members.Select(g => g.ToString()));
        internal set { this.Members = value.Split(Separator).Select(Guid.Parse).ToList(); }
    }

    /// <summary>
    /// Gets or sets list of all members of the lobby.
    /// </summary>
    /// <value>List of users that are members.</value>
    [NotMapped]
    public ICollection<Guid> Members { get; set; } = new List<Guid>();

    /// <summary>
    /// Gets or sets time the lobby was created.
    /// </summary>
    /// <value>Created time.</value>
    required public DateTime CreatedTime { get; set; }
}