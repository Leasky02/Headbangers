using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbyInputHandler : MonoBehaviour
{
    private int m_activeLetterIndex = 0;

    private const int NUM_LETTERS = 3;

    private char[] m_letters = new char[NUM_LETTERS] { 'A', 'A', 'A' };

    public char GetLetter(int index)
    {
        return m_letters[index];
    }

    public void HandleAction_Lobby_NavigateLeft(InputAction.CallbackContext context)
    {
        if (!context.performed)
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

        char letter = m_letters[m_activeLetterIndex];
        char newLetter = ((char)(letter - 1));
        if (newLetter < 'A')
        {
            newLetter = 'Z';
        }
        m_letters[m_activeLetterIndex] = newLetter;
        UpdateAssociatedLobbySpawnPoint();
    }

    private void UpdateAssociatedLobbySpawnPoint()
    {
        LobbyManager lobbyManager = LobbyManager.Find();
        LobbySpawnPoint lobbySpawnPoint = lobbyManager.GetSpawnPointForUserIndex(Player.GetPlayerComponent(gameObject).GetUserIndex());
        lobbySpawnPoint.UpdateDisplayNameText();
    }
}
