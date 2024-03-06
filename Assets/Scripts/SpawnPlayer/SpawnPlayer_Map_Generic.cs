using UnityEngine;

public class SpawnPlayer_Map_Generic : LS.ISpawnPlayer<PlayerConfiguration>
{
    private MultipleTargetCamera m_multiTargetCamera;

    private SpawnPoint[] m_spawnPoints;

    public void Awake()
    {
        m_multiTargetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MultipleTargetCamera>(); // TODO: consider adding Find method on MultipleTargetCamera
        m_spawnPoints = gameObject.GetComponentsInChildren<SpawnPoint>();
    }

    public override void SpawnPlayer(PlayerConfiguration playerConfig)
    {
        Player player = playerConfig.GetPlayer();


        if (player != null)
        {
            // TODO: could we have this happen automatically?
            player.Init(playerConfig.PlayerIndex);

            // TODO: Add function to determine spawn point to use
            SpawnPoint spawnPoint = m_spawnPoints[playerConfig.GetUserIndex()];

            player.SetPosition(spawnPoint.GetPosition());

            player.OnGameStart();

            m_multiTargetCamera.AddTargets(playerConfig.Input.transform);
        }
    }
}
