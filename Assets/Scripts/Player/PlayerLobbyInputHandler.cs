using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyInputHandler : MonoBehaviour
{
    private int m_activeLetterIndex = 0;

    private const int NUM_LETTERS = 3;

    private char[] m_letters = new char[NUM_LETTERS] { 'A', 'A', 'A' };

    private Player player;

    public void Start()
    {
        player = GetComponent<Player>();
        int userIndex = player.GetUserIndex();
        m_letters[0] = (char)(m_letters[0] + userIndex * 3 + 0);
        m_letters[1] = (char)(m_letters[1] + userIndex * 3 + 1);
        m_letters[2] = (char)(m_letters[2] + userIndex * 3 + 2);
        UpdateAssociatedLobbySpawnPoint();
    }

    public char GetLetter(int index)
    {
        return m_letters[index];
    }

    public int GetActiveLetterIndex()
    {
        return m_activeLetterIndex;
    }

    public void HandleAction_Lobby_ReadyUp(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        PlayerConfigurationManager.Instance.TogglePlayerReady(playerIndex);
        PlayerConfigurationManager.Instance.SetPlayerDisplayName(playerIndex, new string(m_letters));

        UpdateAssociatedLobbySpawnPoint();
    }

    public void HandleAction_Lobby_Leave(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        PlayerColorManager.Instance.SetColorAvailable(PlayerConfigurationManager.Instance.GetPlayerColorID(playerIndex));
        PlayerConfigurationManager.Instance.RemovePlayer(playerIndex, gameObject);
    }

    public void HandleAction_Lobby_NavigateLeft(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        if (PlayerConfigurationManager.Instance.IsPlayerReady(playerIndex))
            return;

        m_activeLetterIndex -= 1;
        if (m_activeLetterIndex < 0)
        {
            m_activeLetterIndex = NUM_LETTERS - 1;
        }
        UpdateAssociatedLobbySpawnPoint();
    }

    public void HandleAction_Lobby_NavigateRight(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        if (PlayerConfigurationManager.Instance.IsPlayerReady(playerIndex))
            return;

        m_activeLetterIndex += 1;
        if (m_activeLetterIndex >= NUM_LETTERS)
        {
            m_activeLetterIndex = 0;
        }
        UpdateAssociatedLobbySpawnPoint();
    }

    public void HandleAction_Lobby_NavigateUp(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        if (PlayerConfigurationManager.Instance.IsPlayerReady(playerIndex))
            return;

        char letter = m_letters[m_activeLetterIndex];
        char newLetter = ((char)(letter + 1));
        if (newLetter > 'Z')
        {
            newLetter = 'A';
        }
        m_letters[m_activeLetterIndex] = newLetter;
        UpdateAssociatedLobbySpawnPoint();
    }

    public void HandleAction_Lobby_NavigateDown(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        if (PlayerConfigurationManager.Instance.IsPlayerReady(playerIndex))
            return;

        char letter = m_letters[m_activeLetterIndex];
        char newLetter = ((char)(letter - 1));
        if (newLetter < 'A')
        {
            newLetter = 'Z';
        }
        m_letters[m_activeLetterIndex] = newLetter;
        UpdateAssociatedLobbySpawnPoint();
    }

    public void HandleAction_Lobby_ColorUp(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        if (PlayerConfigurationManager.Instance.IsPlayerReady(playerIndex))
            return;

        if (PlayerColorManager.Instance.HasColorsAvailable())
        {
            int newColorID = PlayerColorManager.Instance.TakeNextAvailableColorID(PlayerConfigurationManager.Instance.GetPlayerColorID(playerIndex));
            player.AssignColor(newColorID);
        }
    }

    public void HandleAction_Lobby_ColorDown(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int playerIndex = player.GetPlayerIndex();
        if (PlayerConfigurationManager.Instance.IsPlayerReady(playerIndex))
            return;

        if (PlayerColorManager.Instance.HasColorsAvailable())
        {
            int newColorID = PlayerColorManager.Instance.TakePreviousAvailableColorID(PlayerConfigurationManager.Instance.GetPlayerColorID(playerIndex));
            player.AssignColor(newColorID);
        }
    }

    private void UpdateAssociatedLobbySpawnPoint()
    {
        LobbyManager lobbyManager = LobbyManager.Find();
        LobbySpawnPoint lobbySpawnPoint = lobbyManager.GetSpawnPointForUserIndex(player.GetUserIndex());
        lobbySpawnPoint.UpdateForUser();
    }
}
