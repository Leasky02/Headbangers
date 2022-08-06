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
        m_userIndex = -1;
        playerNumberText.text = "Player " + playerNumber;
    }

    void Update()
    {
        PlayerConfiguration correspondingPlayer = PlayerConfigurationManager.Instance.GetPlayerConfigurationByUserIndex(playerNumber - 1);
        readyText.text = correspondingPlayer != null && correspondingPlayer.IsReady ? "Ready" : "";
    }

    public void SetReadyText(bool ready)
    {
        Debug.Log(readyText);
        readyText.text = ready ? "Ready" : "";
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void AssignUserIndex(int userIndex)
    {
        m_userIndex = userIndex;
        UpdateDisplayNameText();
    }

    public void UpdateDisplayNameText()
    {
        PlayerConfiguration playerConfig = PlayerConfigurationManager.Instance.GetPlayerConfigurationByUserIndex(m_userIndex);
        PlayerLobbyInputHandler playerLobbyInputHandler = Player.GetPlayerComponent(playerConfig.Input.gameObject).GetComponent<PlayerLobbyInputHandler>();

        for (int i = 0; i < letters.Length; ++i)
        {
            letters[i].text = playerLobbyInputHandler.GetLetter(i).ToString();
        }
    }
}
