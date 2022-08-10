using UnityEngine;

// TODO: cleanup
public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private bool isFoot;
    [SerializeField] private bool isBody;

    [SerializeField] private PlayerActions actionScript;

    [SerializeField] private int objectForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            if ((isFoot && actionScript.IsKicking()) || (isBody && actionScript.IsHeadbutting()))
            {
                ApplyForce(other.gameObject);

                Player.GetPlayerComponent(gameObject).GetComponent<PlayerAudio>().PlayInteractionSound();
            }
        }
    }

    private void ApplyForce(GameObject other)
    {
        Vector3 objectPosition = other.transform.position;
        Vector3 playerPosition = transform.position;
        Vector3 direction = new Vector3(objectPosition.x - playerPosition.x, 0f, objectPosition.z - playerPosition.z);
        direction = direction.normalized;

        other.GetComponent<Rigidbody>().AddForce(direction * objectForce, ForceMode.Impulse);
    }
}
