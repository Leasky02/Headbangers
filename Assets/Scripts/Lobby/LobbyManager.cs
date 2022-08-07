using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
///  This class is responsible for enabling player joining and starting / stopping the countdown when all players are ready
/// </summary>
public class LobbyManager : MonoBehaviour
{
    [SerializeField] MainMenuController mainMenuController;

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

    public void Start()
    {
        gameObject.tag = Tag;
        PlayerConfigurationManager.Instance.EnableJoining();
        InputSystem.onEvent += OnInputEvent;
    }

    public void OnDestroy()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    public void Update()
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

    public void UpdateAllSpawnPoint()
    {
        foreach (LobbySpawnPoint spawnPoint in spawnPoints)
        {
            spawnPoint.UpdateForUser();
        }
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        // Ignore anything that isn't a state event.
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;

        Gamepad gamepad = device as Gamepad;
        if (gamepad == null)
        {
            // Event isn't for a gamepad or device ID is no longer valid.
            return;
        }

        bool buttonEastPressed = gamepad.buttonEast.ReadValueFromEvent(eventPtr) == 1;
        if (buttonEastPressed)
        {
            HandleBackAction();
        }
    }

    private void HandleBackAction()
    {
        if (PlayerConfigurationManager.Instance.GetPlayerCount() == 0)
        {
            mainMenuController.LeaveLobby();
        }
    }
}
