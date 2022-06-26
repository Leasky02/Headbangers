using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDetector : MonoBehaviour
{
    private GameObject objectTouching;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            objectTouching = other.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            objectTouching = null;
        }
    }

    public GameObject IsTouchingBody()
    {
        return objectTouching;
    }
}
