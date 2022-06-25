using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbReplicator : MonoBehaviour
{
    [SerializeField] private GameObject limbToReplilcate;
    [SerializeField] private GameObject activePlayer;
    [SerializeField] private bool isLeg;
    private ConfigurableJoint cj;
    private void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!(isLeg && activePlayer.GetComponent<PlayerMovement>().IsSitting()))
        {
            cj.targetRotation = limbToReplilcate.transform.rotation;
        }
    }
}
