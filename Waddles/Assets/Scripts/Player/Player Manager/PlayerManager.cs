using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    //stores all players in game
    public List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private int requiredPlayers = 1;
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

    private void Update()
    {
        if(roundManager.GetInLobby())
        {
            if (playerConfigs.Count < requiredPlayers)
            {
                roundManager.StopCountdown();
                return;
            }

            foreach (PlayerConfiguration config in playerConfigs)
            {
                if (!config.IsReady)
                {
                    if (roundManager != null)
                    {
                        roundManager.StopCountdown();
                        return;
                    }
                }
            }

            if (!roundManager.GetCountdownInProgress())
                roundManager.PlayersReady();
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
        //if (!playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            playerInput.transform.SetParent(transform);
            PlayerConfiguration playerConfiguration = new PlayerConfiguration(playerInput);
            playerConfigs.Add(playerConfiguration);

            GameObject spawnPlayerObject = GameObject.FindGameObjectWithTag("SpawnPlayer");
            spawnPlayerObject.GetComponent<ISpawnPlayer>().SpawnPlayer(playerConfiguration);
        }
    }

    public void PlayerLeave(PlayerConfiguration playerConfiguration)
    {
        playerConfigs.Remove(playerConfiguration);

        Invoke("ShufflePlayers", 0.05f);
    }

    private void ShufflePlayers()
    {
        //resort players in order
        foreach (PlayerConfiguration config in playerConfigs)
        {
            int oldIndex = config.PlayerIndex;
            config.PlayerIndex = config.Input.user.index;

            //reshuffle position
            GameObject spawnPlayerObject = GameObject.FindGameObjectWithTag("SpawnPlayer");
            spawnPlayerObject.GetComponent<ISpawnPlayer>().ShufflePlayer(config, oldIndex);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawnPlayerObjects = GameObject.FindGameObjectWithTag("SpawnPlayer");
        playerConfigs.ForEach(playerConfigs =>
        {
            spawnPlayerObjects.GetComponent<ISpawnPlayer>().SetupPlayer(playerConfigs);
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