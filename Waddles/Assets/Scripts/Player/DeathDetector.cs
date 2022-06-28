using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    private Transform spawnPoint;

    [SerializeField] private Transform hipParent;

    private void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Vector3 newPosition = new Vector3(spawnPoint.position.x + (Random.Range(-1f, 1f)), spawnPoint.position.y, spawnPoint.position.z + (Random.Range(-1f, 1f)));
        hipParent.position = newPosition;
    }
}
