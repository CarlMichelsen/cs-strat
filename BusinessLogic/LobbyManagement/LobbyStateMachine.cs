using Domain.Lobby;
using Interface.LobbyManagement;

namespace BusinessLogic.LobbyManagement;

/// <inheritdoc />
public class LobbyStateMachine : ILobbyStateMachine
{
    /// <inheritdoc />
    public Dictionary<Guid, Grenade> DistributeGrenades(ActiveLobby activeLobby, List<Grenade> grenades)
    {
        var dict = new Dictionary<Guid, Grenade>();
        var rng = new Random();
        var randomlyOrderedMemberIds = activeLobby.Members
            .Select(d => d.Key)
            .OrderBy(a => rng.Next())
            .ToList();

        var counter = 0;
        while (grenades.Count > 0)
        {
            if (counter >= randomlyOrderedMemberIds.Count)
            {
                break;
            }
            var memberId = randomlyOrderedMemberIds[counter];

            var grenadeToAsign = grenades.First();
            activeLobby.Members[memberId].GrenadeAsignment = grenadeToAsign;
            dict.Add(memberId, grenadeToAsign);
            grenades.Remove(grenadeToAsign);
        }

        return dict;
    }

    /// <inheritdoc />
    public MetaUser? UserDisconnected(ActiveLobby activeLobby, User user)
    {
        if (activeLobby.Members.TryGetValue(user.Id, out var metaUser))
        {
            metaUser.Online = false;
            return metaUser;
        }

        return default;
    }
}
