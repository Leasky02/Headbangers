using System.Collections;
using UnityEngine;

public class PlayerKO : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private AudioSource audioSource_KO;

    [SerializeField] private float deadSpringValue;

    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackHeightVelocity;

    [SerializeField] private float minimumDamage;
    [SerializeField] private float maximumDamage;
    [SerializeField] private float minimumAngle;
    [SerializeField] private float maximumAngle;

    [SerializeField] private float kickingDamage;

    [SerializeField] private float startingKnockoutTime;
    [SerializeField] private float knockoutTime;
    [SerializeField] private float graceTime;
    [SerializeField] private float knockoutReduceTimeDelay;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

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

    // TODO: move elsewhere
    public void KnockBack(Vector3 knockedBackFrom, bool knockBackAtHead = false)
    {
        Vector3 selfPosition = transform.position;
        Vector3 direction = new Vector3(selfPosition.x - knockedBackFrom.x, 0f, selfPosition.z - knockedBackFrom.z);
        direction = direction.normalized;

        if (knockBackAtHead)
        {
            Vector3 forcePosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            rb.AddForceAtPosition(direction * knockBackForce * 0.4f, forcePosition, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(direction * knockBackForce, ForceMode.Impulse);
        }

        direction.y = knockBackHeightVelocity;
        rb.velocity = new Vector3(direction.x, direction.y, direction.z);
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

    public void PlayKnockOutSound()
    {
        audioSource_KO.pitch = Random.Range(0.8f, 1.2f);
        audioSource_KO.Play();
    }
}
