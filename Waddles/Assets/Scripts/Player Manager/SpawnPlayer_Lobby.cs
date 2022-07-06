using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer_Lobby : ISpawnPlayer
{
    //spawnpositions
    [SerializeField] private Transform[] spawnPositions;

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        Debug.Log(playerConfig.PlayerIndex);
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        SetColor(playerConfig);
    }

    public override void SetupPlayer(PlayerConfiguration playerConfig)
    {
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
    }

    private void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.PlayerIndex].position;
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
