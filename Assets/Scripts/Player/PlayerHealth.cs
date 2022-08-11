using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int initialHealth = 100;
    [SerializeField] private int minRecoveryHealth = 55;
    [SerializeField] private int maxRecoveryHealth = 100;
    [SerializeField] private int recoveryHealthReduction = 15;
    [SerializeField] private float healthRegenerateTimeDelay = 1;

    public float Health { get; private set; }

    private int maxHealth;

    private int recoveryHealth;

    public void Init()
    {
        Health = initialHealth;
        maxHealth = initialHealth;
        recoveryHealth = maxRecoveryHealth;

        StartCoroutine(RegenerateHealth());
    }

    // TODO: call on recover
    // Regenerates health regularly
    private IEnumerator RegenerateHealth()
    {
        if (!IsKnockedOut())
        {
            GainHealth(1);

            yield return new WaitForSeconds(healthRegenerateTimeDelay);
            StartCoroutine(RegenerateHealth());
        }

    }

    public void ReduceHealth(float amount)
    {
        Health = Mathf.Round(Health - amount);
        if (Health <= 0)
        {
            Health = 0;
        }
    }

    public void GainHealth(int amount)
    {
        Health += amount;
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
    }

    public void SetHealth(int amount)
    {
        Health = Mathf.Clamp(amount, 0, maxHealth);
    }

    public void Recover()
    {
        // Recovery health reduces with each recovery, making it easier to be knocked out each time.
        SetHealth(recoveryHealth);
        DecreaseRecoveryHealth(-recoveryHealthReduction);
    }

    public void SetRecoveryHealth(int amount)
    {
        recoveryHealth = Mathf.Clamp(amount, minRecoveryHealth, maxRecoveryHealth);
    }

    public void IncreaseRecoveryHealth(int amount)
    {
        SetRecoveryHealth(recoveryHealth + amount);
    }

    public void DecreaseRecoveryHealth(int amount)
    {
        SetRecoveryHealth(recoveryHealth - amount);
    }

    // TODO: move elsewhere
    private bool IsKnockedOut()
    {
        return Player.GetPlayerComponent(gameObject).IsKnockedOut();
    }
}
