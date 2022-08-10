
public class IGameSetupFactory
{
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
}