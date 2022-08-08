
public class Gameplay : Singleton<Gameplay>
{
    public IPlayerRespawn PlayerRespawn { get; private set; }

    public void Init()
    {
        // TODO: initialise somewhere else and pass in
        PlayerRespawn = new IPlayerRespawn();
        PlayerRespawn.Init();
    }
}