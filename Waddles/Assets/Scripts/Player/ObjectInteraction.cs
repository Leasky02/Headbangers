using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private bool isFoot;
    [SerializeField] private bool isBody;

    [SerializeField] private PlayerActions actionScript;
    [SerializeField] private AudioSource attackAudioSource;

    [SerializeField] private int objectForce;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            if ((isFoot && actionScript.IsKicking()) || (isBody && actionScript.IsAttemptingHeadButt()))
            {
                ApplyForce(other.gameObject);

                attackAudioSource.pitch = Random.Range(0.8f, 1.4f);
                attackAudioSource.Play();
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