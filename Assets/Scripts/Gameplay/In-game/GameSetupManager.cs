using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSetupManager : MonoBehaviour
{
    private int randomMapIndex;
    private int randomGameModeIndex;

    private string[] gameModeNames;

    // Start is called before the first frame update
    void Start()
    {
        gameModeNames = new string[] {"FreeForAll", "Tag"};
        SetupGame();
    }

    private void SetupGame()
    {
        SelectAndSetupRandomGame();
        // SelectRandomMap();
    }

    // private void SelectRandomMap()
    // {
    //     randomMapIndex = Random.Range(0, mapNames.Length);
    //     mapPreviews[randomMapIndex].SetActive(true);
    //     Invoke("QueueLoadMap", 12f);
    // }

    private void SelectAndSetupRandomGame()
    {
        //TEMPORARILY set to default game mode
        randomGameModeIndex = 0;

        string typeString = ComposeGameSetupFactory(gameModeNames[randomGameModeIndex]);
        System.Type type = System.Type.GetType(typeString);

        Gameplay.Instance.Setup((IGameSetupFactory)System.Activator.CreateInstance(type));
    }

    // private void QueueLoadMap()
    // {
    //     LoadMap(mapNames[randomMapIndex]);
    // }

    // private void LoadMap(string mapName)
    // {
    //     PlayerConfigurationManager.Instance.SwitchCurrentActionMap("Deactive");
    //     SceneManager.LoadScene(mapName);
    // }

    private string ComposeGameSetupFactory(string gameModeName)
    {
        Debug.Log(gameModeName);
        return "GameSetupFactory_" + gameModeName;
    }
}