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
        SetAsCameraTarget(playerConfig.Input.transform);
        //Hi Calum, the positions dont change from lobby to start of game, this function
        //should only be used in team games where teams spawn at opposite sides
        SetPosition(playerConfig);
        FindRespawnPoint(playerConfig);
        UnFreezePosition(playerConfig);
    }

    private void SetAsCameraTarget(Transform playerTransform)
    {
        playerCamera.AddTargets(playerTransform.transform);
    }

    private void SetPosition(PlayerConfiguration playerConfig)
    {
        Transform playerTransform = playerConfig.Input.transform.GetChild(0);
        playerTransform.position = spawnPositions[playerConfig.GetUserIndex()].position;
    }
    public void UnFreezePosition(PlayerConfiguration playerConfig)
    {
        Rigidbody playerHip = playerConfig.Input.gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
        playerHip.constraints = RigidbodyConstraints.None;
    }

    private void FindRespawnPoint(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerDeath>().FindRespawnPoint();
    }

}
