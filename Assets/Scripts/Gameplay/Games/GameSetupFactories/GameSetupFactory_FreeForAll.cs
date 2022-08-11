
public class GameSetupFactory_FreeForAll : IGameSetupFactory
{
    public override IPlayerKickedHandler CreatePlayerKickedHandler()
    {
        return new PlayerKickedHandler_FreeForAll();
    }
}