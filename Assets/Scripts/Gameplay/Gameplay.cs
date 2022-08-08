using UnityEngine.SceneManagement;

public class Gameplay : Singleton<Gameplay>
{
    public bool InGame { get; private set; }

    public IPlayerRespawn PlayerRespawn { get; private set; }

    public IPlayerKickedHandler PlayerKickedHandler { get; private set; }

    public void Start()
    {
        InGame = false;
    }

    public void Setup(IGameSetupFactory gameSetupFactory)
    {
        PlayerRespawn = gameSetupFactory.CreatePlayerRespawn();
        PlayerKickedHandler = gameSetupFactory.CreatePlayerKickedHandler();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!InGame)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            PlayerRespawn.InitOnSceneLoad();
            PlayerKickedHandler.InitOnSceneLoad();

            InGame = true;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // We want to tear everything down when we are leaving a game
        if (InGame)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            PlayerRespawn = null;
            PlayerKickedHandler = null;

            InGame = false;
        }
    }
}