using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    //stores all players in game
    [SerializeField]private List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();
    //default colours for the player
    [SerializeField] private Color[] playerColours; 
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Shoultn't set singleton");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (!playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            playerInput.transform.SetParent(transform);
            PlayerConfiguration playerConfiguration = new PlayerConfiguration(playerInput);
            playerConfigs.Add(playerConfiguration);

            GameObject spawnPlayerObject = GameObject.FindGameObjectWithTag("SpawnPlayer");
            spawnPlayerObject.GetComponent<ISpawnPlayer>().SpawnPlayer(playerConfiguration);

            //set colour
            playerInput.GetComponent<PlayerData>().SetPlayerColor(playerColours[playerConfiguration.PlayerIndex]);
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        playerConfigs.Add(new PlayerConfiguration(playerInput));
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawnPlayerObjects = GameObject.FindGameObjectWithTag("SpawnPlayer");
        playerConfigs.ForEach(playerConfigs =>
        {
            spawnPlayerObjects.GetComponent<ISpawnPlayer>().SpawnPlayer(playerConfigs);
        });
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        Input = pi;
        PlayerIndex = pi.playerIndex;
    }

    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public string Team { get; set; }
}