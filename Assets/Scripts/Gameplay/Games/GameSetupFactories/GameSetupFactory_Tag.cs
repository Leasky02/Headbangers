
public class GameSetupFactory_Tag : IGameSetupFactory
{
    public override IPlayerKickedHandler CreatePlayerKickedHandler()
    {
        return new PlayerKickedHandler_Tag();
    }
}