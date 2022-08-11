
public class PlayerKickedHandler_Tag : IPlayerKickedHandler
{
    // TODO: improve
    public override void HandleKicked(Player kickedPlayer, Player kickingPlayer)
    {
        if (!kickedPlayer.CanBeHeadbutted())
            return;

        GameState_Tag gameState = ((GameState_Tag)Gameplay.Instance.GameState);

        if (gameState.GetTaggedPlayer() == kickedPlayer)
        {
            // TODO: don't return, just prevent damage done
            return;
        }

        if (gameState.GetTaggedPlayer() == kickingPlayer)
        {
            // TODO: don't return, just prevent damage done
            return;
        }

        base.HandleKicked(kickedPlayer, kickingPlayer);
    }
}
