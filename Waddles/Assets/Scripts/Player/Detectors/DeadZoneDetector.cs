using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneDetector : MonoBehaviour
{
    [SerializeField] private Transform hipParent;
    [SerializeField] private PlayerDeath playerDeath;

    private int _playerIndex;

    //when collides with dead zone beneath map
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            PlayerConfigurationManager.Instance.GetPlayerState(_playerIndex).SetDead(true);
            StartCoroutine(playerDeath.Die());
        }
    }
}
