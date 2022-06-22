using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedDetector : MonoBehaviour
{
    [HideInInspector] public bool isGrounded = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("LANDED");
            isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
