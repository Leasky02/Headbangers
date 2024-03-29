using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ConfigurableJoint cj;
    [SerializeField] private int speed = 2000;

    private bool isWalking = false;
    private Vector2 normalizedInputDirection;

    // Fixed Update is called 30x per frame
    public void FixedUpdate()
    {
        if (CanWalk())
        {
            Vector3 moveDirection = new Vector3(normalizedInputDirection.x, 0f, normalizedInputDirection.y);

            isWalking = (moveDirection.sqrMagnitude > 0);
            if (isWalking)
            {
                Walk(moveDirection);
            }
            else
            {
                moveDirection = Vector3.zero;
            }
        }
    }

    public void Update()
    {
        if (ShouldIdle())
        {
            GetComponent<PlayerDecoyAnimator>().PlayIdleAnimation();
        }
    }

    public void SetMoveDirection(Vector2 normalizedDirection)
    {
        normalizedInputDirection = normalizedDirection;
    }

    private void Walk(Vector3 direction)
    {
        bool isGrounded = GetComponent<Player>().IsGrounded();
        Vector3 walkingForce = direction * speed * Time.deltaTime;
        // TODO: confirm logic with Alasdair

        // Move the player
        rb.AddForce(walkingForce);

        // Rotate the player
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(-direction.x, direction.y, direction.z), Vector3.up);
        cj.targetRotation = toRotation;

        // Play walking animation
        GetComponent<PlayerDecoyAnimator>().PlayWalkAnimation(isGrounded);
    }

    private bool CanWalk()
    {
        return !GetComponent<PlayerKick>().IsKicking() && !GetComponent<PlayerSit>().IsSitting() && !GetComponent<PlayerHeadbutt>().IsHeadbutting() && !GetComponent<Player>().IsKnockedOut();
    }

    private bool ShouldIdle()
    {
        return !GetComponent<PlayerKick>().IsKicking() && !GetComponent<PlayerSit>().IsSitting() && !isWalking && !GetComponent<PlayerHeadbutt>().IsHeadbutting();
    }

    public void ResetMovementDirection()
    {
        
    }
}
