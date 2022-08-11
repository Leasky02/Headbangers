using UnityEngine;

public class PlayerSit : MonoBehaviour
{
    private bool isSitting = false;

    public void AttemptSit()
    {
        if (CanSit())
        {
            isSitting = true;
            GetComponent<PlayerDecoyAnimator>().PlaySitAnimation();
        }
    }

    public void StopSitting()
    {
        isSitting = false;
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
