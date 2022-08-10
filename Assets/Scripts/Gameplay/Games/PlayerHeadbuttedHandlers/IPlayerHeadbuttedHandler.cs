using UnityEngine;

public class IPlayerHeadbuttedHandler
{

    public void InitOnSceneLoad()
    {
    }

    public virtual void HandleHeadbutted(Player headbuttedPlayer, Player headbuttingPlayer)
    {
        Debug.Log("Player " + headbuttedPlayer);
    }
}
