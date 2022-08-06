using UnityEngine;

public class LobbySpawnPoint : MonoBehaviour
{
    [SerializeField] private int playerNumber;

    [SerializeField] private TextMesh readyText;

    [SerializeField] private TextMesh playerNumberText;

    void Start()
    {
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
}
