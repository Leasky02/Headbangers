using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer_FreeForAll : LS.ISpawnPlayer<PlayerConfiguration>
{
    //camera
    [SerializeField] private MultipleTargetCamera playerCamera;
    //spawnpositions
    [SerializeField] private Transform[] spawnPositions;

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        //Debug.Log(playerConfig.Input.user.index);
        SetAsCameraTarget(playerConfig.Input.transform);
        SetPosition(playerConfig);
    }

    public override void SetupPlayer(PlayerConfiguration playerConfig)
    {
        SetAsCameraTarget(playerConfig.Input.transform);
        SetPosition(playerConfig);
        FindRespawnPoint(playerConfig);
    }

    private void SetAsCameraTarget(Transform playerTransform)
    {
        playerCamera.AddTargets(playerTransform.transform);
    }

    private void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.Input.user.index].position;
    }

    private void FindRespawnPoint(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerDeath>().FindRespawnPoint();
    }

}
