using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer_FreeForAll : ISpawnPlayer
{
    //camera
    [SerializeField] private MultipleTargetCamera playerCamera;
    //spawnpositions
    [SerializeField] private Transform[] spawnPositions;

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        Debug.Log(playerConfig.PlayerIndex);
        SetAsCamerTarget(playerConfig.Input.transform);
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
    }

    public override void SetupPlayer(PlayerConfiguration playerConfig)
    {
        SetAsCamerTarget(playerConfig.Input.transform);
        SetPosition(playerConfig);
        SetInputMap(playerConfig);
        FindRespawnPoint(playerConfig);
    }

    private void SetAsCamerTarget(Transform playerTransform)
    {
        playerCamera.AddTargets(playerTransform.transform);
    }

    private void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.PlayerIndex].position;
    }
    private void SetInputMap(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.SwitchCurrentActionMap("Gameplay");
    }
    private void FindRespawnPoint(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerDeath>().FindRespawnPoint();
    }

}
