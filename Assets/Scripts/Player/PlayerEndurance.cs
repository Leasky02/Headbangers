using UnityEngine;

// TODO: consider renaming to PlayerKnockOut?
public class PlayerEndurance : MonoBehaviour
{
    [SerializeField] private float startingKnockoutTime = 3;
    [SerializeField] private float graceTime = 2;
    [SerializeField] private float knockoutReduceTimeDelay = 8;

    private float knockoutTime;

    public void Start()
    {
        knockoutTime = startingKnockoutTime;
        StartIncreasingEndurance();
    }

    public void Update()
    {
        if (IsEnduranceIncreasing())
        {
            if (IsKnockedOut())
                StopIncreasingEndurance();
        }
        else
        {
            if (!IsKnockedOut())
                StartIncreasingEndurance();
        }
    }

    private void StartIncreasingEndurance()
    {
        InvokeRepeating("ImproveEndurance", 0, knockoutReduceTimeDelay);
    }

    private void StopIncreasingEndurance()
    {
        CancelInvoke("ImproveEndurance");
    }

    private bool IsEnduranceIncreasing()
    {
        return IsInvoking("ImproveEndurance");
    }

    // Improves knockout time and increases starting amount of health after waking up
    public void ImproveEndurance()
    {
        ReduceKnockoutTime();
        IncreaseRecoveryHealth();
    }

    private void ReduceKnockoutTime()
    {
        if (knockoutTime > startingKnockoutTime)
            knockoutTime -= 0.1f;
    }

    private void IncreaseRecoveryHealth()
    {
        PlayerHealth playerHealth = Player.GetPlayerComponent(gameObject).GetComponent<PlayerHealth>();
        playerHealth.IncreaseRecoveryHealth(2);
    }

    private bool IsKnockedOut()
    {
        return Player.GetPlayerComponent(gameObject).IsKnockedOut();
    }

    // TEMP

    public float GetGraceTime()
    {
        return graceTime;
    }

    public float GetKnockOutTime()
    {
        return knockoutTime;
    }
}
