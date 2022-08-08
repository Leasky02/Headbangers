using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour
{
    [SerializeField] string[] mapNames;

    [SerializeField] string[] gameModeNames;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetupGame", 1f);
    }

    private void SetupGame()
    {
        SelectAndSetupRandomGame();
        SelectRandomMap();
    }

    private void SelectRandomMap()
    {
        int randomMapIndex = Random.Range(0, mapNames.Length);
        LoadMap(mapNames[randomMapIndex]);
    }

    private void SelectAndSetupRandomGame()
    {
        int randomGameModeIndex = Random.Range(0, gameModeNames.Length);
        string typeString = ComposeGameSetupFactory(gameModeNames[randomGameModeIndex]);
        System.Type type = System.Type.GetType(typeString);

        Gameplay.Instance.Setup((IGameSetupFactory)System.Activator.CreateInstance(type));
    }

    private void LoadMap(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }

    private string ComposeGameSetupFactory(string gameModeName)
    {
        Debug.Log(gameModeName);
        return "GameSetupFactory_" + gameModeName;
    }
}
