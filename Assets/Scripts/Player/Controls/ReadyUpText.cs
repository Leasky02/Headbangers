using UnityEngine;

public class ReadyUpText : MonoBehaviour
{
    [SerializeField] private int ID;
    [SerializeField] private TextMesh readyUpText;

    void Update()
    {
        PlayerConfiguration correspondingPlayer = PlayerConfigurationManager.Instance.GetPlayerConfigurationByUserIndex(ID);
        readyUpText.text = correspondingPlayer != null && correspondingPlayer.IsReady ? "Ready" : "";
    }
}
