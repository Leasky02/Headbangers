using UnityEngine;

public class SpawnPlayer_FreeForAll : LS.ISpawnPlayer<PlayerConfiguration>
{
    [SerializeField] private MultipleTargetCamera playerCamera;

    [SerializeField] private Transform[] spawnPositions;

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        SetAsCameraTarget(playerConfig.Input.transform);
        //Hi Calum, the positions dont change from lobby to start of game, this function
        //should only be used in team games where teams spawn at opposite sides
        SetPosition(playerConfig);
        FindRespawnPoint(playerConfig);
        UnFreezePosition(playerConfig);

        Player.GetPlayerComponent(playerConfig.Input.gameObject).OnGameStart();
    }

    private void SetAsCameraTarget(Transform playerTransform)
    {
        playerCamera.AddTargets(playerTransform.transform);
    }

    private void SetPosition(PlayerConfiguration playerConfig)
    {
        Player.GetPlayerComponent(playerConfig.Input.gameObject).SetPosition(spawnPositions[playerConfig.GetUserIndex()].position);
    }
    public void UnFreezePosition(PlayerConfiguration playerConfig)
    {
        Player.GetPlayerComponent(playerConfig.Input.gameObject).GetRigidbodyHip().constraints = RigidbodyConstraints.None;
    }

    private void FindRespawnPoint(PlayerConfiguration playerConfig)
    {
        playerConfig.Input.transform.GetChild(0).GetComponent<PlayerDeath>().FindRespawnPoint();
    }

}
