using System.Collections;
using UnityEngine;

public class PlayerKO : MonoBehaviour
{
    [SerializeField] private float minimumDamage = 10;
    [SerializeField] private float maximumDamage = 40;
    [SerializeField] private float minimumAngle = 0;
    [SerializeField] private float maximumAngle = 75;

    [SerializeField] private float kickingDamage = 10;

    [SerializeField] private float startingKnockoutTime = 3;
    [SerializeField] private float graceTime = 2;
    [SerializeField] private float knockoutReduceTimeDelay = 8;

    private float knockoutTime;


    // Start is called before the first frame update
    void Start()
    {
        knockoutTime = startingKnockoutTime;

        StartCoroutine(ImproveEndurance());
    }

    public void KnockOut(GameObject knockedOutBy = null)
    {
        ConfigurableJoint[] bodyParts = gameObject.GetComponentsInChildren<ConfigurableJoint>();
        foreach (ConfigurableJoint bodyPart in bodyParts)
        {
            JointDrive springDriveX = bodyPart.angularXDrive;
            JointDrive springDriveYZ = bodyPart.angularYZDrive;

            springDriveX.positionSpring /= 100;
            springDriveYZ.positionSpring /= 100;

            bodyPart.angularXDrive = springDriveX;
            bodyPart.angularYZDrive = springDriveYZ;
        }
    }

    public void Revive()
    {
        ConfigurableJoint[] bodyParts = gameObject.GetComponentsInChildren<ConfigurableJoint>();
        foreach (ConfigurableJoint bodyPart in bodyParts)
        {
            JointDrive springDriveX = bodyPart.angularXDrive;
            JointDrive springDriveYZ = bodyPart.angularYZDrive;

            springDriveX.positionSpring *= 100;
            springDriveYZ.positionSpring *= 100;

            bodyPart.angularXDrive = springDriveX;
            bodyPart.angularYZDrive = springDriveYZ;
        }
    }

    // TODO: use invoke
    //improves knockout time and increases starting amount of health after waking up
    public IEnumerator ImproveEndurance()
    {
        if (!IsKnockedOut())
        {
            ReduceKnockoutTime();
            IncreaseRecoveryHealth();

            yield return new WaitForSeconds(knockoutReduceTimeDelay);

            StartCoroutine(ImproveEndurance());
        }
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

    public float GetKickingDamage()
    {
        return kickingDamage;
    }

    public float GetGraceTime()
    {
        return graceTime;
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

    public float GetKnockOutTime()
    {
        return knockoutTime;
    }
}
