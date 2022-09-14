
public class PlayerHeadbuttedHandler_Tag : IPlayerHeadbuttedHandler
{
    // TODO: improve
    public override void HandleHeadbutted(Player headbuttedPlayer, Player headbuttingPlayer)
    {
        if (!headbuttedPlayer.CanBeHeadbutted())
            return;

        GameState_Tag gameState = ((GameState_Tag)Gameplay.Instance.GameState);

        if (gameState.GetTaggedPlayer() == headbuttedPlayer)
        {
            // TODO: don't return, just prevent damage done
            return;
        }

        if (gameState.GetTaggedPlayer() == headbuttingPlayer)
        {
            gameState.TagPlayer(headbuttedPlayer);
            // TODO: don't return, just prevent damage done
            return;
        }

        base.HandleHeadbutted(headbuttedPlayer, headbuttingPlayer);
    }
}
