using UnityEngine;

public class DeadZoneDetector : MonoBehaviour
{
    [SerializeField] private PlayerDeath playerDeath;

    //when collides with dead zone beneath map
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            // TODO: Add IPlayerDeathHandler script
            StartCoroutine(playerDeath.Die());
        }
    }
}
