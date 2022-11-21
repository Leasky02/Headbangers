using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour
{
    [SerializeField] string[] mapNames;

    [SerializeField] string[] gameModeNames;

    [SerializeField] private GameObject[] mapPreviews;

    [SerializeField] private TMP_Text roundNumberDisplay;
    [SerializeField] private TextMesh roundNumberDisplayElevator;

    [SerializeField] private TMP_Text gameModeDisplay;
    [SerializeField] private TMP_Text mapNameDisplay;
    [SerializeField] private TextMesh mapNameDisplayElevator;

    int randomMapIndex;
    int randomGameModeIndex;

    // Start is called before the first frame update
    void Start()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        SelectAndSetupRandomGame();
        SelectRandomMap();

        //ERROR trying to access round manager instance??
        roundNumberDisplay.text = ("Round " + RoundManager.Instance.GetRoundNumber().ToString());
        roundNumberDisplayElevator.text = ("" + RoundManager.Instance.GetRoundNumber().ToString());
    }

    private void SelectRandomMap()
    {
        randomMapIndex = Random.Range(0, mapNames.Length);
        mapPreviews[randomMapIndex].SetActive(true);
        Invoke("QueueLoadMap", 12f);
    }

    private void SelectAndSetupRandomGame()
    {
        randomGameModeIndex = Random.Range(0, gameModeNames.Length);
        string typeString = ComposeGameSetupFactory(gameModeNames[randomGameModeIndex]);
        System.Type type = System.Type.GetType(typeString);

        Gameplay.Instance.Setup((IGameSetupFactory)System.Activator.CreateInstance(type));
    }

    public void RevealText()
    {
        gameModeDisplay.text = ("" + gameModeNames[randomGameModeIndex]);
        mapNameDisplay.text = ("" + mapNames[randomMapIndex]);
        mapNameDisplayElevator.text = ("" + mapNames[randomMapIndex]);
    }

    private void QueueLoadMap()
    {
        LoadMap(mapNames[randomMapIndex]);
    }

    private void LoadMap(string mapName)
    {
        PlayerConfigurationManager.Instance.SwitchCurrentActionMap("Deactive");
        SceneManager.LoadScene(mapName);
    }

    private string ComposeGameSetupFactory(string gameModeName)
    {
        //Debug.Log(gameModeName);
        return "GameSetupFactory_" + gameModeName;
    }
}
