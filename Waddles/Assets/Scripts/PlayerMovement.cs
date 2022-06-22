using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private ConfigurableJoint cj;

    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;
    [SerializeField] private int rotationSpeed;

    [SerializeField] private Animator decoyAnimator;

    [SerializeField] private GroundedDetector groundDetector;

    private bool isWalking = false;
    private bool justJumped = false;
    [HideInInspector] public bool isSitting = false;

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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //move the player
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 lookDirection = new Vector3(-horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        rb.AddForce(movementDirection * speed * Time.deltaTime);

        //if player is moving
        if (movementDirection != Vector3.zero && !isSitting)
        {
            //rotate the player
            Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            cj.targetRotation = toRotation;

            isWalking = true;
            decoyAnimator.Play("Walk");
        }
        else
        {
            isWalking = false;
        }

        //sit
        if(Input.GetButton("Sit") && !isWalking)
        {
            isSitting = true;

            decoyAnimator.Play("Sit");
        }
        else
        {
            isSitting = false;
        }
        //if not moving or sitting
        if(!isSitting && !isWalking)
        {
            decoyAnimator.Play("Idle");
        }

        //jump and not sitting
        if(Input.GetButtonDown("Jump") && !isSitting)
        {
            //if on ground
            if (groundDetector.isGrounded && !justJumped)
            {
                StartCoroutine("Jump");
            }
        }
    }
    private IEnumerator Jump()
    {
        justJumped = true;
        rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);
        justJumped = false;
    }
}
