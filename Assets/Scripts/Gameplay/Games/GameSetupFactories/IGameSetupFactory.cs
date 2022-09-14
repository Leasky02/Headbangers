
public class IGameSetupFactory
{
    public virtual IGameState CreateGameState()
    {
        return new IGameState();
    }

    public virtual IPlayerRespawn CreatePlayerRespawn()
    {
        return new IPlayerRespawn();
    }

    public virtual IPlayerKnockedOutHandler CreatePlayerKnockedOutHandler()
    {
        return new IPlayerKnockedOutHandler();
    }

    public virtual IPlayerKickedHandler CreatePlayerKickedHandler()
    {
        return new IPlayerKickedHandler();
    }

    public virtual IPlayerHeadbuttedHandler CreatePlayerHeadbuttedHandler()
    {
        return new IPlayerHeadbuttedHandler();
    }
}