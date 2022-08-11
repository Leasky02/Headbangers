using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private bool isFoot;
    [SerializeField] private bool isBody;
    [SerializeField] private int objectForce;

    private Player player;

    public void Start()
    {
        player = Player.GetPlayerComponent(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            if ((isFoot && player.GetComponent<PlayerKick>().IsKicking()) || (isBody && player.GetComponent<PlayerHeadbutt>().IsHeadbutting()))
            {
                ApplyForce(other.gameObject);

                player.GetComponent<PlayerAudio>().PlayInteractionSound();
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
