using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKO : MonoBehaviour
{
    private PlayerActions actionScript;
    private ConfigurableJoint cj;
    private Rigidbody rb;
    private AudioSource myAudioSource;

    [SerializeField] private AudioClip knockOut_CLIP;

    private bool attackInProgress = false;
    private bool knockedOut = false;

    [SerializeField] private float deadSpringValue;

    [SerializeField] private float rotationForce;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackHeightVelocity;

    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] private float minimumDamage;
    [SerializeField] private float maximumDamage;
    [SerializeField] private float minimumAngle;
    [SerializeField] private float maximumAngle;

    [SerializeField] private float startingKnockoutTime;
    [SerializeField] private float knockoutTime;
    [SerializeField] private float graceTime;
    [SerializeField] private float healthRegenerateTimeDelay;
    [SerializeField] private float knockoutReduceTimeDelay;

    // Start is called before the first frame update
    void Start()
    {
        actionScript = GetComponent<PlayerActions>();
        cj = GetComponent<ConfigurableJoint>();
        rb = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();

        currentHealth = startingHealth;
        knockoutTime = startingKnockoutTime;

        StartCoroutine(RegenerateHealth());
        StartCoroutine(ReduceKnockoutTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Attacked(GameObject attackingPlayer, float angle)
    {
        attackInProgress = true;

        if (!actionScript.IsAttemptingAttack())
        {
            if (angle > 75f)
                angle = 75f;
            if (angle < 0f)
                angle = 0f;

            Vector2 pointA = new Vector2(minimumAngle, minimumDamage);
            Vector2 pointB = new Vector2(maximumAngle, maximumDamage);
            float m = ((pointA.y - pointB.y) / (pointA.x - pointB.x));
            float c = pointA.y - m * pointA.x;
            float damage = m * angle + c;

            currentHealth = Mathf.Round(currentHealth - damage);
        }

        if (currentHealth <= 0 && !knockedOut)
        {
            currentHealth = 70f;

            knockedOut = true;

            myAudioSource.clip = knockOut_CLIP;
            myAudioSource.pitch = Random.Range(0.8f, 1.2f);
            myAudioSource.Play();

            Vector3 attackerPosition = attackingPlayer.transform.position;
            Vector3 selfPosition = transform.position;
            Vector3 direction = new Vector3(selfPosition.x - attackerPosition.x, 0f, selfPosition.z - attackerPosition.z);
            direction.Normalize();

            Vector3 forcePosition = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
            rb.AddForceAtPosition(direction * rotationForce, forcePosition, ForceMode.Impulse);

            direction.y = knockbackHeightVelocity;
            rb.velocity = new Vector3(direction.x * knockbackForce, 10f, direction.z * knockbackForce);


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

            knockoutTime += 0.4f;
            knockedOut = false;

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
            StartCoroutine(ReduceKnockoutTime());

            yield return new WaitForSeconds(graceTime);

            attackInProgress = false;
        }
        else
        {
            yield return new WaitForSeconds(graceTime/2f);
            attackInProgress = false;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        if (!knockedOut)
        {
            if (currentHealth < startingHealth)
                currentHealth ++;

            yield return new WaitForSeconds(healthRegenerateTimeDelay);
            StartCoroutine(RegenerateHealth());
        }
    }

    private IEnumerator ReduceKnockoutTime()
    {
        if (!knockedOut)
        {
            if (knockoutTime > startingKnockoutTime)
                knockoutTime -= 0.1f;

            yield return new WaitForSeconds(knockoutReduceTimeDelay);
            StartCoroutine(ReduceKnockoutTime());
        }
    }

    public bool IsAttackInProgress()
    {
        return attackInProgress;
    }

    public bool IsKnockedOut()
    {
        return knockedOut;
    }
}
