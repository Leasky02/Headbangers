using UnityEngine;

// TODO: move some logic into body class

public class PlayerSit : MonoBehaviour
{
    [SerializeField] private Animator decoyAnimator;

    private bool isSitting = false;

    public void AttemptSit()
    {
        if (CanSit())
        {
            isSitting = true;
            PlaySitAnimation();
        }
    }

    public void StopSitting()
    {
        isSitting = false;
    }

    private void PlaySitAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("Sit");
    }

    private bool CanSit()
    {
        return !GetComponent<PlayerKick>().IsKicking() && !GetComponent<Player>().IsKnockedOut();
    }

    public bool IsSitting()
    {
        return isSitting;
    }
}
