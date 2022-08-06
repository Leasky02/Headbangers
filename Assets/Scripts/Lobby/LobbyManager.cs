using UnityEngine;

/// <summary>
///  This class is responsible for enabling player joining and starting / stopping the countdown when all players are ready
/// </summary>
public class LobbyManager : MonoBehaviour
{
    [SerializeField] private int requiredPlayers = 1;

    [SerializeField] private LobbySpawnPoint[] spawnPoints;

    private const string Tag = "LobbyManager";

    public static LobbyManager Find()
    {
        LobbyManager lobbyManager = GameObject.FindGameObjectWithTag(LobbyManager.Tag).GetComponent<LobbyManager>();
        if (!lobbyManager)
        {
            Debug.LogError("lobbyManager does not exist");
            return null;
        }
        return lobbyManager;
    }

    void Start()
    {
        gameObject.tag = Tag;
        PlayerConfigurationManager.Instance.EnableJoining();
    }

    void Update()
    {
        if (RoundManager.Instance.IsCountdownInProgress())
        {
            if (!PlayerConfigurationManager.Instance.AllPlayersReady()
              || PlayerConfigurationManager.Instance.GetPlayerCount() < requiredPlayers)
            {
                RoundManager.Instance.StopCountdown();
            }
        }
        else
        {
            if (PlayerConfigurationManager.Instance.AllPlayersReady()
              && PlayerConfigurationManager.Instance.GetPlayerCount() >= requiredPlayers)
            {
                RoundManager.Instance.PlayersReady();
            }
        }
    }

    public LobbySpawnPoint GetSpawnPointForUserIndex(int userIndex)
    {
        return spawnPoints[userIndex];
    }
}
