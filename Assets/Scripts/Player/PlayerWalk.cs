using System.Collections;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ConfigurableJoint cj;
    [SerializeField] private Animator decoyAnimator;
    [SerializeField] private GroundedDetector groundDetector;
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
        }
    }

    public void Update()
    {
        if (ShouldIdle())
        {
            PlayIdleAnimation();
        }
    }

    public void SetMoveDirection(Vector2 normalizedDirection)
    {
        normalizedInputDirection = normalizedDirection;
    }

    private void Walk(Vector3 direction)
    {
        Vector3 walkingForce = direction * speed * Time.deltaTime;
        // TODO: confirm logic with Alasdair
        if (!IsDead() || (IsDead() && !groundDetector.IsGrounded()))
        {
            // Do nothing
        }
        else if (!IsDead() && !groundDetector.IsGrounded())
        {
            walkingForce *= 3;
        }
        else
        {
            walkingForce *= 2;
        }

        // Move the player
        rb.AddForce(walkingForce);

        // Rotate the player
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(-direction.x, direction.y, direction.z), Vector3.up);
        cj.targetRotation = toRotation;

        PlayWalkAnimation();
    }

    private void PlayIdleAnimation()
    {
        decoyAnimator.Play("Idle");
    }

    private void PlayWalkAnimation()
    {
        if (!IsDead())
        {
            decoyAnimator.Play("Walk");
        }

        if (groundDetector.IsGrounded())
            decoyAnimator.speed = 1f;
        else
            decoyAnimator.speed = 0.5f;
    }

    private bool IsDead()
    {
        return GetComponent<Player>().IsDead();
    }

    private bool CanWalk()
    {
        return !GetComponent<PlayerKick>().IsKicking() && !GetComponent<PlayerSit>().IsSitting() && !GetComponent<PlayerHeadbutt>().IsHeadbutting() && !GetComponent<Player>().IsKnockedOut();
    }

    private bool ShouldIdle()
    {
        return !GetComponent<PlayerKick>().IsKicking() && !GetComponent<PlayerSit>().IsSitting() && !isWalking && !GetComponent<PlayerHeadbutt>().IsHeadbutting();
    }
}
