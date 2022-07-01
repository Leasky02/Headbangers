using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    [SerializeField] private Transform hipParent;
    [SerializeField] private PlayerDeath playerDeath;

    [SerializeField] private PlayerData playerData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
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
