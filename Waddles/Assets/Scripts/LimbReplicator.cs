using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbReplicator : MonoBehaviour
{
    [SerializeField] private GameObject limbToReplilcate;
    private ConfigurableJoint cj;
    private void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cj.targetRotation = limbToReplilcate.transform.rotation;
    }
}
