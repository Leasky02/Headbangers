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
            //temporary reversing of death state
            //will change to just kill
            if (playerData.GetDead())
            {
                playerData.SetDead(false);
                playerDeath.Revive();
            }
            else
            {
                playerData.SetDead(true);
                playerDeath.Die();
            }
        }
    }
}
