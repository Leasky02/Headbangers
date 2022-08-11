using UnityEngine.SceneManagement;

public class Gameplay : Singleton<Gameplay>
{
    public bool InGame { get; private set; }

    public IGameState GameState { get; private set; }

    public IPlayerRespawn PlayerRespawn { get; private set; }

    public IPlayerKnockedOutHandler PlayerKnockedOutHandler { get; private set; }

    public IPlayerKickedHandler PlayerKickedHandler { get; private set; }

    public IPlayerHeadbuttedHandler PlayerHeadbuttedHandler { get; private set; }

    public void Start()
    {
        InGame = false;
    }

    public void Setup(IGameSetupFactory gameSetupFactory)
    {
        GameState = gameSetupFactory.CreateGameState();
        PlayerRespawn = gameSetupFactory.CreatePlayerRespawn();
        PlayerKnockedOutHandler = gameSetupFactory.CreatePlayerKnockedOutHandler();
        PlayerKickedHandler = gameSetupFactory.CreatePlayerKickedHandler();
        PlayerHeadbuttedHandler = gameSetupFactory.CreatePlayerHeadbuttedHandler();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!InGame)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            GameState.InitOnSceneLoad();
            PlayerRespawn.InitOnSceneLoad();
            PlayerKnockedOutHandler.InitOnSceneLoad();
            PlayerKickedHandler.InitOnSceneLoad();
            PlayerHeadbuttedHandler.InitOnSceneLoad();

            InGame = true;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // We want to tear everything down when we are leaving a game
        if (InGame)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            GameState = null;
            PlayerRespawn = null;
            PlayerKnockedOutHandler = null;
            PlayerKickedHandler = null;
            PlayerHeadbuttedHandler = null;

            InGame = false;
        }
    }

    public void OnGameStart()
    {
        PlayerConfigurationManager.Instance.SwitchCurrentActionMap("Gameplay");

        GameState.OnGameStart();
    }
}