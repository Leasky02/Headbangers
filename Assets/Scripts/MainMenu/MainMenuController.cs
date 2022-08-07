using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private GameObject mainMenuView;

    [SerializeField] private GameObject lobbyView;

    void Awake()
    {
        mainMenuView.SetActive(true);
        lobbyView.SetActive(false);
        SelectButtonOnScreen(mainMenuView);
    }

    public void GoToLobby()
    {
        SwitchView(mainMenuView, lobbyView);
        PlayerConfigurationManager.Instance.EnableJoining(); // TODO: remove from here
    }

    public void LeaveLobby()
    {
        PlayerConfigurationManager.Instance.DisableJoining(); // TODO: remove from here
        SwitchView(lobbyView, mainMenuView);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }

    private void SwitchView(GameObject viewToClose, GameObject viewToOpen)
    {
        viewToClose.SetActive(false);
        viewToOpen.SetActive(true);
        SelectButtonOnScreen(viewToOpen);
    }

    private void SelectButtonOnScreen(GameObject screen)
    {
        Button firstButton = screen.GetComponentInChildren<Button>();
        if (firstButton)
        {
            eventSystem.SetSelectedGameObject(firstButton.gameObject);
        }
    }
}
