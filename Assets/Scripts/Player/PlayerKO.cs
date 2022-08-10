using System.Collections;
using UnityEngine;

public class PlayerKO : MonoBehaviour
{
    [SerializeField] private float minimumDamage = 10;
    [SerializeField] private float maximumDamage = 40;
    [SerializeField] private float minimumAngle = 0;
    [SerializeField] private float maximumAngle = 75;

    [SerializeField] private float kickingDamage = 10;

    private bool IsKnockedOut()
    {
        return Player.GetPlayerComponent(gameObject).IsKnockedOut();
    }

    // TEMP

    public float GetKickingDamage()
    {
        return kickingDamage;
    }

    public float GetMinimumAngle()
    {
        return minimumAngle;
    }

    public float GetMaximumAngle()
    {
        return maximumAngle;
    }

    public float GetMinimumDamage()
    {
        return minimumDamage;
    }

    public float GetMaximumDamage()
    {
        return maximumDamage;
    }
}
