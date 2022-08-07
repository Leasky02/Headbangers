using System.Collections;
using UnityEngine;

public class PlayerKO : MonoBehaviour
{
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

    //called when headbutted
    public IEnumerator HeadButted(GameObject headButtingPlayer, float angle)
    {
        headButtInProgress = true;

        //calculate damage
        bool fightingBack = actionScript.IsAttemptingHeadButt();
        if (!fightingBack)
        {
            currentHealth = Mathf.Round(currentHealth - CalculateDamage(angle));
        }
        else
        {
            currentHealth = Mathf.Round(currentHealth - (CalculateDamage(angle) * 0.5f));
        }

        //determine if KO'd
        bool shouldKO = currentHealth <= 0 && !IsKnockedOut();
        if (shouldKO)
        {
            StartCoroutine(KO(headButtingPlayer));
        }
        else
        {
            Knockback(headButtingPlayer);
            ChangeFace();

            yield return new WaitForSeconds(graceTime / 2f);
            headButtInProgress = false;
        }
    }

    public void Kicked(GameObject headButtingPlayer)
    {
        currentHealth -= kickingDamage;

        if (currentHealth <= 0 && !IsKnockedOut())
        {
            StartCoroutine(KO(headButtingPlayer));
        }
        else
        {
            Knockback(headButtingPlayer);
            ChangeFace();
        }
    }

    //change face based on current health
    private void ChangeFace()
    {
        if (Player.GetPlayerComponent(gameObject).IsKnockedOut())
        {
            if (currentHealth > 50)
            {
                StartCoroutine(face.ChangeEmotion("sad", "open", "sad", 2f));
            }
            else if (currentHealth > 0)
            {
                StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 3f));
            }
        }
    }

    //knocked out
    private IEnumerator KO(GameObject headButtingPlayer)
    {
        currentHealth = recoveryHealth;
        recoveryHealth = Mathf.Clamp(recoveryHealth - recoveryHealthReduction, minRecoveryHealth, maxRecoveryHealth);

        SetKnockedOut(true);
        face.KnockedOut();

        audioSource_KO.pitch = Random.Range(0.8f, 1.2f);
        audioSource_KO.Play();

        Vector3 headButterPosition = headButtingPlayer.transform.position;
        Vector3 selfPosition = transform.position;
        Vector3 direction = new Vector3(selfPosition.x - headButterPosition.x, 0f, selfPosition.z - headButterPosition.z);
        direction = direction.normalized;

        Vector3 forcePosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        rb.AddForceAtPosition(direction * knockbackForce * 0.4f, forcePosition, ForceMode.Impulse);

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
        SetKnockedOut(false);
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

    //knocked back
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

    //calculates damage when headbutted
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

    //regenerates health regularly
    private IEnumerator RegenerateHealth()
    {
        if (!IsKnockedOut())
        {
            if (currentHealth < startingHealth)
                currentHealth++;

            yield return new WaitForSeconds(healthRegenerateTimeDelay);
            StartCoroutine(RegenerateHealth());
        }
    }

    //improves knockout time and increases starting amount of health after waking up
    private IEnumerator ImproveEndurance()
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
        recoveryHealth = Mathf.Clamp(recoveryHealth + 2, minRecoveryHealth, maxRecoveryHealth);
    }

    //returns if player is being currently headbutted
    public bool IsHeadButtInProgress()
    {
        return headButtInProgress;
    }

    private bool IsKnockedOut()
    {
        return Player.GetPlayerComponent(gameObject).IsKnockedOut();
    }

    private void SetKnockedOut(bool knockedOut)
    {
        Player.GetPlayerComponent(gameObject).SetKnockedOut(knockedOut);
    }
}
