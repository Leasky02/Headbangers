using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayer_Lobby : LS.ISpawnPlayer<PlayerConfiguration>
{
    //spawnpositions
    [SerializeField] private Transform[] spawnPositions;

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        Debug.Log(playerConfig.PlayerIndex);
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        SetColor(playerConfig);
        SetReadyUp(playerConfig);
        FreezePosition(playerConfig);
    }
    public override void ShufflePlayer(PlayerConfiguration playerConfig)
    {
        int userIndex = playerConfig.GetUserIndex();
        TextMesh oldText = spawnPositions[userIndex].GetChild(0).GetComponent<TextMesh>();
        oldText.text = "";

        TextMesh readyText = spawnPositions[userIndex].GetChild(0).GetComponent<TextMesh>();
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().SetupReadyUp(readyText, playerConfig);
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().TransferReadyUp();

        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        SetReadyUp(playerConfig);
    }

    private void SetReadyUp(PlayerConfiguration playerConfig)
    {
        TextMesh readyText = spawnPositions[playerConfig.GetUserIndex()].GetChild(0).GetComponent<TextMesh>();
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().SetupReadyUp(readyText, playerConfig);
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

    private void SetColor(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.gameObject.transform.GetChild(0).gameObject.GetComponent<PlayerColor>().DefaultColor(playerConfig);
    }
}
