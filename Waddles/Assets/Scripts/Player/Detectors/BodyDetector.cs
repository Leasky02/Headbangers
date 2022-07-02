using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDetector : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private GameObject objectTouching;
    [SerializeField] private GameObject self;

    private void OnTriggerEnter(Collider other)
    {
        //collision with player
        bool touchingOtherPlayer = other.CompareTag("Body") && other.gameObject != self && !playerData.GetDead();
        if (touchingOtherPlayer)
        {
            objectTouching = other.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //exit collision with player
        bool touchingOtherPlayer = other.CompareTag("Body") && other.gameObject != self && !playerData.GetDead();
        if (touchingOtherPlayer)
        {
            objectTouching = null;
        }
    }

    public GameObject IsTouchingBody()
    {
        return objectTouching;
    }
}
