using UnityEngine;

public class PlayerKickedHandler_Tag : IPlayerKickedHandler
{
    public override void HandleKicked(Player kickedPlayer, Player kickingPlayer)
    {
        Debug.Log("Player " + kickedPlayer);
    }
}
