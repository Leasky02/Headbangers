using UnityEngine;

public class MainMenuViewController : MonoBehaviour
{
    [SerializeField] MainMenuController mainMenuController;

    [SerializeField] MenuButton3D[] menuButtons;

    public void OnPlay()
    {
        mainMenuController.GoToLobby();
    }

    public void OnAchievements()
    {
        Debug.Log("OnAchievements");
    }

    public void OnOptions()
    {
        Debug.Log("OnOptions");
    }

    public void OnQuit()
    {
        mainMenuController.QuitGame();
    }
}
