using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    [SerializeField] private Transform hipParent;
    [SerializeField] private PlayerDeath playerDeath;

    [SerializeField] private PlayerData playerData;

    //when collides with dead zone beneath map
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            playerData.SetDead(true);
            playerDeath.Die();
        }
    }
}
