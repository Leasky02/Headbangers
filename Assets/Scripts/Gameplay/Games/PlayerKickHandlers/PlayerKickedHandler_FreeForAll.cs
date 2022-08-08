using UnityEngine;

public class PlayerKickedHandler_FreeForAll : IPlayerKickedHandler
{
    public override void HandleKicked(Player kickedPlayer, Player kickingPlayer)
    {
        Debug.Log("Player " + kickedPlayer);
    }
}
