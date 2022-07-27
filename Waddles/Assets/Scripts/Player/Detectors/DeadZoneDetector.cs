using UnityEngine;

public class DeadZoneDetector : MonoBehaviour
{
    [SerializeField] private Transform hipParent;
    [SerializeField] private PlayerDeath playerDeath;

    //when collides with dead zone beneath map
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            Player.GetPlayerComponent(gameObject).SetDead(true);
            StartCoroutine(playerDeath.Die());
        }
    }
}
