using UnityEngine;

public class PlayerDecoyAnimator : MonoBehaviour
{
    [SerializeField] private Animator decoyAnimator;

    public void PlayIdleAnimation()
    {
        decoyAnimator.Play("Idle");
    }

    public void PlayWalkAnimation(bool isGrounded)
    {
        decoyAnimator.Play("Walk");
        decoyAnimator.speed = isGrounded ? 1f : 0.5f;
    }

    public void PlayKickAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("Kick");
    }

    public void PlayHeadbuttAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("HeadButt");
    }

    public void PlaySitAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("Sit");
    }
}
