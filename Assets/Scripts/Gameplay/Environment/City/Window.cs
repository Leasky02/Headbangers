using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private float deletionRadius = 10f;
    [SerializeField] private Vector3 centrePoint;

    [SerializeField] private float minTransparency = 0f;
    [SerializeField] private float maxTransparency = 1f;

    [SerializeField] private Color color;
    private MeshRenderer mr;
    private bool isOn;
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

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
        float positionZ = transform.position.z;

        if (positionY < 2f ||                                       // Check if it is underground
            (positionX >= 10f && (rotationY > 250f || rotationY <= 40f)) ||  // Check left side
            (positionX <= -10f && (rotationY < 110 || rotationY >= 330f)) ||   // Check right side
            (positionX > -10f && positionX < 10f && (rotationY > 300f || rotationY <= 60f ||  // Check middle
            (positionX > -10f && positionX < 10f && positionZ > -30f || (rotationY > 300f || rotationY <= 60f)))))  // Check middle front
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

    public void InvokeTurnOn(float delay)
    {
        Invoke("TurnOn", delay);
    }

    public void InvokeTurnOff(float delay)
    {
        Invoke("TurnOff", delay);
    }

    public void TurnOn()
    {
        mr.enabled = true;
        isOn = true;
    }
    public void TurnOff()
    {
        mr.enabled = false;
        isOn = false;
    }

    public bool GetIsOn()
    {
        return isOn;
    }
}
