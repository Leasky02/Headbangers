using UnityEngine;

public class IPlayerKnockedOutHandler
{

    public void InitOnSceneLoad()
    {
    }

    public virtual void HandleKnockedOut(Player player, GameObject knockedOutBy = null)
    {
        // TODO
        // if (!player.IsKnockedOut())
        // {
        //     PlayerKO playerKO = player.GetComponentInChildren<PlayerKO>(); // TODO: get from component, not children
        //     playerKO.KnockOut(knockedOutBy);
        // }
    }
}
