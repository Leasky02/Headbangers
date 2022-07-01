using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKO : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private PlayerActions actionScript;
    private ConfigurableJoint cj;
    private Rigidbody rb;
    [SerializeField] private AudioSource audioSource_KO;

    private bool headButtInProgress = false;

    [SerializeField] private float deadSpringValue;

    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackHeightVelocity;

    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;
    private int recoveryHealth;
    [SerializeField] private int minRecoveryHealth;
    [SerializeField] private int maxRecoveryHealth;
    [SerializeField] private int recoveryHealthReduction;

    [SerializeField] private float minimumDamage;
    [SerializeField] private float maximumDamage;
    [SerializeField] private float minimumAngle;
    [SerializeField] private float maximumAngle;

    [SerializeField] private float kickingDamage;

    [SerializeField] private float startingKnockoutTime;
    [SerializeField] private float knockoutTime;
    [SerializeField] private float graceTime;
    [SerializeField] private float healthRegenerateTimeDelay;
    [SerializeField] private float knockoutReduceTimeDelay;

    [SerializeField] private FacialEmotions face;

    // Start is called before the first frame update
    void Start()
    {
        actionScript = GetComponent<PlayerActions>();
        cj = GetComponent<ConfigurableJoint>();
        rb = GetComponent<Rigidbody>();

        currentHealth = startingHealth;
        knockoutTime = startingKnockoutTime;
        recoveryHealth = maxRecoveryHealth;

        StartCoroutine(RegenerateHealth());
        StartCoroutine(ImproveEndurance());
    }

    public IEnumerator HeadButted(GameObject headButtingPlayer, float angle)
    {
        headButtInProgress = true;

        bool fightingBack = actionScript.IsAttemptingHeadButt();
        if (!fightingBack)
        {
            currentHealth = Mathf.Round(currentHealth - CalculateDamage(angle));
        }
        else
        {
            currentHealth = Mathf.Round(currentHealth - (CalculateDamage(angle) * 0.5f));
        }

        bool shouldKO = currentHealth <= 0 && !playerData.GetKnockedOut();
        if (shouldKO)
        {
            StartCoroutine(KO(headButtingPlayer));
        }
        else
        {
            Knockback(headButtingPlayer);
            ChangeFace();

            yield return new WaitForSeconds(graceTime/2f);
            headButtInProgress = false;
        }
    }

    public void Kicked(GameObject headButtingPlayer)
    {
        currentHealth -= kickingDamage;

        if(currentHealth <= 0 && !playerData.GetKnockedOut())
        {
            StartCoroutine(KO(headButtingPlayer));
        }
        else
        {
            Knockback(headButtingPlayer);
            ChangeFace();
        }
    }

    private void ChangeFace()
    {
        if(currentHealth > 50)
            StartCoroutine(face.ChangeEmotion("sad", "open", "sad", 2f));
        else if(currentHealth > 0)
            StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 3f));
    }

    private IEnumerator KO(GameObject headButtingPlayer)
    {
        currentHealth = recoveryHealth;
        recoveryHealth = Mathf.Clamp(recoveryHealth - recoveryHealthReduction, minRecoveryHealth, maxRecoveryHealth);

        playerData.SetKnockedOut(true);
        face.KnockedOut();

        audioSource_KO.pitch = Random.Range(0.8f, 1.2f);
        audioSource_KO.Play();

        Vector3 headButterPosition = headButtingPlayer.transform.position;
        Vector3 selfPosition = transform.position;
        Vector3 direction = new Vector3(selfPosition.x - headButterPosition.x, 0f, selfPosition.z - headButterPosition.z);
        direction = direction.normalized;

        Vector3 forcePosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        rb.AddForceAtPosition(direction * knockbackForce * 0.7f, forcePosition, ForceMode.Impulse);

        direction.y = knockbackHeightVelocity;
        rb.velocity = new Vector3(direction.x, direction.y, direction.z);


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

        yield return new WaitForSeconds(knockoutTime);

        knockoutTime += 0.5f;
        playerData.SetKnockedOut(false);
        face.Revived();

        foreach (ConfigurableJoint bodyPart in bodyParts)
        {
            JointDrive springDriveX = bodyPart.angularXDrive;
            JointDrive springDriveYZ = bodyPart.angularYZDrive;

            springDriveX.positionSpring *= 100;
            springDriveYZ.positionSpring *= 100;

            bodyPart.angularXDrive = springDriveX;
            bodyPart.angularYZDrive = springDriveYZ;
        }

        StartCoroutine(RegenerateHealth());
        StartCoroutine(ImproveEndurance());

        yield return new WaitForSeconds(graceTime);

        headButtInProgress = false;
    }

    private void Knockback(GameObject headButtingPlayer)
    {
        Vector3 headButterPosition = headButtingPlayer.transform.position;
        Vector3 selfPosition = transform.position;
        Vector3 direction = new Vector3(selfPosition.x - headButterPosition.x, 0f, selfPosition.z - headButterPosition.z);
        direction = direction.normalized;

        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        direction.y = knockbackHeightVelocity;
        rb.velocity = new Vector3(direction.x, direction.y, direction.z);
    }

    private float CalculateDamage(float angle)
    {
        angle = Mathf.Clamp(angle, 0f, 75f);

        Vector2 pointA = new Vector2(minimumAngle, minimumDamage);
        Vector2 pointB = new Vector2(maximumAngle, maximumDamage);
        float m = ((pointA.y - pointB.y) / (pointA.x - pointB.x));
        float c = pointA.y - m * pointA.x;
        float damage = m * angle + c;
        return damage;
    }

    private IEnumerator RegenerateHealth()
    {
        if (!playerData.GetKnockedOut())
        {
            if (currentHealth < startingHealth)
                currentHealth ++;

            yield return new WaitForSeconds(healthRegenerateTimeDelay);
            StartCoroutine(RegenerateHealth());
        }
    }

    private IEnumerator ImproveEndurance()
    {
        if (!playerData.GetKnockedOut())
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
        recoveryHealth = Mathf.Clamp(recoveryHealth + 2, minRecoveryHealth, maxRecoveryHealth);
    }

    public bool IsHeadButtInProgress()
    {
        return headButtInProgress;
    }
}
