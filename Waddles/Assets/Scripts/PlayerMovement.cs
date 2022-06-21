using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private ConfigurableJoint cj;

    [SerializeField] private int speed;
    [SerializeField] private int rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cj = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get input direction and move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 lookDirection = new Vector3(-horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        rb.AddForce(movementDirection * speed * Time.deltaTime);

        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            cj.targetRotation = toRotation;
        }
    }
}
