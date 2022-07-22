using UnityEngine;

/// <summary>
///  This class is responsible for enabling player joining and starting / stopping the countdown when all players are ready
/// </summary>
public class Lobby : MonoBehaviour
{
    [SerializeField] private int requiredPlayers = 1;

    void Start()
    {
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
}
