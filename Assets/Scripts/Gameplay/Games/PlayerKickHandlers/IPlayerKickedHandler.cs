using UnityEngine;

public class IPlayerKickedHandler
{

    public void InitOnSceneLoad()
    {
    }

    public virtual void HandleKicked(Player kickedPlayer, Player kickingPlayer)
    {
        Debug.Log("Player " + kickedPlayer);
    }
}
