
public class GameSetupFactory_FreeForAll : IGameSetupFactory
{
    public override IGameState CreateGameState()
    {
        return new GameState_FreeForAll();
    }

    public override IPlayerKickedHandler CreatePlayerKickedHandler()
    {
        return new PlayerKickedHandler_FreeForAll();
    }
}