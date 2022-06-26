using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDetector : MonoBehaviour
{
    private GameObject objectTouching;
    [SerializeField] private GameObject self;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body") && other.gameObject != self)
        {
            objectTouching = other.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Body") && other.gameObject != self)
        {
            objectTouching = null;
        }
    }

    public GameObject IsTouchingBody()
    {
        return objectTouching;
    }
}
