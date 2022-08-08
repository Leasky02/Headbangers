using System.Collections;
using UnityEngine;

public class IPlayerRespawn
{
    private RespawnPoint[] m_respawnPoints;

    public void Init()
    {
        GameObject spawnPlayerObj = GameObject.FindGameObjectWithTag("SpawnPlayer");
        m_respawnPoints = spawnPlayerObj.GetComponentsInChildren<RespawnPoint>();
    }

    public void RespawnPlayer(Player player)
    {
        Gameplay.Instance.StartCoroutine(Respawn(player));
    }

    private IEnumerator Respawn(Player player)
    {
        yield return new WaitForSeconds(1f);

        Vector3 respawnPosition = GetRespawnPoint(player).GetPosition();

        player.SetPosition(new Vector3(respawnPosition.x + (Random.Range(-1f, 1f)), respawnPosition.y, respawnPosition.z + (Random.Range(-1f, 1f))));

        yield return new WaitForSeconds(0.5f);

        player.SetIsCameraTarget(true);

        PlayerConfigurationManager.Instance.SwitchCurrentActionMap(player.GetPlayerIndex(), "Gameplay");
    }

    protected virtual RespawnPoint GetRespawnPoint(Player player)
    {
        return GetRandomRespawnPoint(player);
    }

    protected virtual RespawnPoint GetRandomRespawnPoint(Player player)
    {
        int respawnPointIndex = Random.Range(0, m_respawnPoints.Length);
        return m_respawnPoints[respawnPointIndex];
    }
}
