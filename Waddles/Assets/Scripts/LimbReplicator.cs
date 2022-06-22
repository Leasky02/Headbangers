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
        
        if(isLeg)
        {
            if(activePlayer.GetComponent<PlayerMovement>().isSitting)
            {
                return;
            }
            else
            {
                cj.targetRotation = limbToReplilcate.transform.rotation;
            }
        }
        else
        {
            cj.targetRotation = limbToReplilcate.transform.rotation;
        }
    }
}
