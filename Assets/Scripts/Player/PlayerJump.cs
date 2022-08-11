using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int jumpForce = 45;
    [SerializeField] private const int jumpLimit = 1;

    private int jumpCount = 0;

    public void AttemptJump()
    {
        if (CanJump())
        {
            bool isGrounded = GetComponent<Player>().IsGrounded();
            if (isGrounded)
            {
                jumpCount = 0;
            }

            if (isGrounded || (jumpCount > 0 && jumpCount < jumpLimit))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private bool CanJump()
    {
        return !GetComponent<PlayerKick>().IsKicking() && !GetComponent<PlayerSit>().IsSitting() && !GetComponent<Player>().IsKnockedOut();
    }
}
