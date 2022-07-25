using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayer_Lobby : LS.ISpawnPlayer<PlayerConfiguration>
{
    //spawnpositions
    [SerializeField] private Transform[] spawnPositions;

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        int playerIndex = playerConfig.PlayerIndex;
        PlayerInput playerInput = playerConfig.Input;
        Debug.Log(playerConfig.PlayerIndex);
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        SetColor(playerIndex, playerInput);
        SetReadyUp(playerConfig);
        FreezePosition(playerConfig);
    }
    public override void ShufflePlayer(PlayerConfiguration playerConfig)
    {
        int userIndex = playerConfig.GetUserIndex();
        TextMesh oldText = spawnPositions[userIndex].GetChild(0).GetComponent<TextMesh>();
        oldText.text = "";

        TextMesh readyText = spawnPositions[userIndex].GetChild(0).GetComponent<TextMesh>();
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().SetupReadyUp(playerConfig);

        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        SetReadyUp(playerConfig);
    }

    private void SetReadyUp(PlayerConfiguration playerConfig)
    {
        TextMesh readyText = spawnPositions[playerConfig.GetUserIndex()].GetChild(0).GetComponent<TextMesh>();
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().SetupReadyUp(playerConfig);
    }

    public void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.GetUserIndex()].position;
    }
    public void FreezePosition(PlayerConfiguration playerConfig)
    {
        Rigidbody playerHip = playerConfig.Input.gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
        playerHip.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void SetInputMap(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.SwitchCurrentActionMap("Lobby");
    }

    private void SetColor(int playerIndex, PlayerInput playerInput)
    {
        playerInput.gameObject.transform.GetChild(0).gameObject.GetComponent<PlayerColor>().DefaultColor(playerIndex);
    }
}
