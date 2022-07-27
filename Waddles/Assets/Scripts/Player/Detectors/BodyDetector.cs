using UnityEngine;

public class BodyDetector : MonoBehaviour
{
    private GameObject objectTouching;
    [SerializeField] private GameObject self;

    private void OnTriggerEnter(Collider other)
    {
        //collision with player
        bool touchingOtherPlayer = other.CompareTag("Body") && other.gameObject != self && !IsDead();
        if (touchingOtherPlayer)
        {
            objectTouching = other.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //exit collision with player
        bool touchingOtherPlayer = other.CompareTag("Body") && other.gameObject != self && !IsDead();
        if (touchingOtherPlayer)
        {
            objectTouching = null;
        }
    }

    public GameObject IsTouchingBody()
    {
        return objectTouching;
    }

    private bool IsDead()
    {
        return Player.GetPlayerComponent(gameObject).IsDead();
    }
}
