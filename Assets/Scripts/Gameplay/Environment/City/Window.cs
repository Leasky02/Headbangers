using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private float deletionRadius = 10f;
    [SerializeField] private Vector3 centrePoint;

    [SerializeField] private float minTransparency = 0f;
    [SerializeField] private float maxTransparency = 1f;

    [SerializeField] private Color color;
    void Start()
    {
        DecideRemoval();
        RandomiseColor();
    }

    void DecideRemoval()
    {
        Vector3 positionWithoutY = new Vector3(transform.position.x, 0f, transform.position.z);
        float distanceFromCenter = Vector3.Distance(centrePoint, positionWithoutY);

        if (distanceFromCenter > deletionRadius)
        {
            Destroy(gameObject);
        }

        float rotationY = transform.rotation.eulerAngles.y;
        float positionX = transform.position.x;
        float positionY = transform.position.y;

        if (positionY < 2f ||                                       // Check if it is underground
            (positionX >= 10f && (rotationY > 250f || rotationY <= 40f)) ||  // Check left side
            (positionX <= -10f && (rotationY < 110 || rotationY >= 330f)) ||   // Check right side
            (positionX > -10f && positionX < 10f && (rotationY > 300f || rotationY <= 60f)))  // Check middle
        {
            Destroy(gameObject);
        }
    }

    void RandomiseColor()
    {
        // Generate random alpha
        color.a = Random.Range(minTransparency, maxTransparency);

        GetComponent<MeshRenderer>().material.color = color;
    }
}
