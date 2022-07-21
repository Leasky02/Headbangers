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
    }
    public override void ShufflePlayer(PlayerConfiguration playerConfig, int oldIndex)
    {
        TextMesh oldText = spawnPositions[oldIndex].GetChild(0).GetComponent<TextMesh>();
        oldText.text = "";

        TextMesh readyText = spawnPositions[playerConfig.Input.user.index].GetChild(0).GetComponent<TextMesh>();
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().SetupReadyUp(readyText, playerConfig);
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().TransferReadyUp();

        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        SetReadyUp(playerConfig);
    }

    public override void SetupPlayer(PlayerConfiguration playerConfig)
    {
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
    }

    private void SetReadyUp(PlayerConfiguration playerConfig)
    {
        TextMesh readyText = spawnPositions[playerConfig.Input.user.index].GetChild(0).GetComponent<TextMesh>();
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerReadyUp>().SetupReadyUp(readyText, playerConfig);
    }

    public void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.Input.user.index].position;
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
