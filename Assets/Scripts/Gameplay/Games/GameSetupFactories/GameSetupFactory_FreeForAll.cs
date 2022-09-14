
public class GameSetupFactory_FreeForAll : IGameSetupFactory
{
    public override IGameState CreateGameState()
    {
        return new GameState_FreeForAll();
    }
}