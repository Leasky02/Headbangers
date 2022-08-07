using TMPro;
using UnityEngine;

public class LobbySpawnPoint : MonoBehaviour
{
    [SerializeField] private int playerNumber;

    [SerializeField] private TextMesh readyText;

    [SerializeField] private TextMesh playerNumberText;

    [SerializeField] private TextMeshPro[] letters;

    private int m_userIndex;

    void Start()
    {
        m_userIndex = playerNumber - 1;
        playerNumberText.text = "Player " + playerNumber;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void UpdateForUser()
    {
        UpdateReadyUpText();
        UpdateDisplayNameText();
    }

    private void UpdateDisplayNameText()
    {
        PlayerConfiguration playerConfig = PlayerConfigurationManager.Instance.GetPlayerConfigurationByUserIndex(m_userIndex);

        if (playerConfig == null)
        {
            for (int i = 0; i < letters.Length; ++i)
            {
                letters[i].text = "_";
                letters[i].color = new Color(255, 255, 255);
                letters[i].gameObject.SetActive(false);
            }
            return;
        }

        PlayerLobbyInputHandler playerLobbyInputHandler = Player.GetPlayerComponent(playerConfig.Input.gameObject).GetComponent<PlayerLobbyInputHandler>();

        for (int i = 0; i < letters.Length; ++i)
        {
            letters[i].text = playerLobbyInputHandler.GetLetter(i).ToString();
            letters[i].color = (playerLobbyInputHandler.GetActiveLetterIndex() == i && !playerConfig.IsReady) ? new Color(0, 255, 0) : new Color(255, 255, 255);
        }
    }

    private void UpdateReadyUpText()
    {
        PlayerConfiguration playerConfig = PlayerConfigurationManager.Instance.GetPlayerConfigurationByUserIndex(m_userIndex);
        readyText.text = playerConfig != null && playerConfig.IsReady ? "Ready" : "";
    }
}
