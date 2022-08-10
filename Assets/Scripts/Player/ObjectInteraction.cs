using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private bool isFoot;
    [SerializeField] private bool isBody;
    [SerializeField] private int objectForce;

    private PlayerActions actions;

    public void Start()
    {
        actions = Player.GetPlayerComponent(gameObject).GetComponent<PlayerActions>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            if ((isFoot && actions.IsKicking()) || (isBody && actions.IsHeadbutting()))
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
