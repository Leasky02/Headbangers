
public class GameSetupFactory_Tag : IGameSetupFactory
{
    public override IGameState CreateGameState()
    {
        return new GameState_Tag();
    }

    public override IPlayerKickedHandler CreatePlayerKickedHandler()
    {
        return new PlayerKickedHandler_Tag();
    }

    public override IPlayerHeadbuttedHandler CreatePlayerHeadbuttedHandler()
    {
        return new PlayerHeadbuttedHandler_Tag();
    }
}