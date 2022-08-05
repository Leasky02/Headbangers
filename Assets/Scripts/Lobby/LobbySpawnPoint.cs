using UnityEngine;

public class LobbySpawnPoint : MonoBehaviour
{
    [SerializeField] private int playerNumber;

    [SerializeField] private TextMesh readyUpText;

    [SerializeField] private TextMesh playerNumberText;

    void Start()
    {
        playerNumberText.text = "Player " + playerNumber;
    }

    void Update()
    {
        PlayerConfiguration correspondingPlayer = PlayerConfigurationManager.Instance.GetPlayerConfigurationByUserIndex(playerNumber - 1);
        readyUpText.text = correspondingPlayer != null && correspondingPlayer.IsReady ? "Ready" : "";
    }
}
