using System.Collections;
using UnityEngine;

public class PlayerKO : MonoBehaviour
{
    private PlayerActions actionScript;
    private ConfigurableJoint cj;
    private Rigidbody rb;
    [SerializeField] private AudioSource audioSource_KO;

    [SerializeField] private float deadSpringValue;

    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackHeightVelocity;

    [SerializeField] private float minimumDamage;
    [SerializeField] private float maximumDamage;
    [SerializeField] private float minimumAngle;
    [SerializeField] private float maximumAngle;

    [SerializeField] private float kickingDamage;

    [SerializeField] private float startingKnockoutTime;
    [SerializeField] private float knockoutTime;
    [SerializeField] private float graceTime;
    [SerializeField] private float knockoutReduceTimeDelay;

    private bool m_canBeHit = true;

    // Start is called before the first frame update
    void Start()
    {
        actionScript = GetComponent<PlayerActions>();
        cj = GetComponent<ConfigurableJoint>();
        rb = GetComponent<Rigidbody>();

        knockoutTime = startingKnockoutTime;

        StartCoroutine(ImproveEndurance());
    }

    public void HeadButted(GameObject headButtingPlayer, float angle)
    {
        PreventFromBeingHit();

        //calculate damage
        bool fightingBack = actionScript.IsAttemptingHeadButt();
        PlayerHealth playerHealth = Player.GetPlayerComponent(gameObject).GetComponent<PlayerHealth>();
        if (!fightingBack)
        {
            playerHealth.ReduceHealth(CalculateDamage(angle));

        }
        else
        {
            playerHealth.ReduceHealth(CalculateDamage(angle) * 0.5f);
        }

        //determine if KO'd
        // TODO: handle KO elsewhere
        bool shouldKO = playerHealth.Health <= 0 && !IsKnockedOut();
        if (shouldKO)
        {
            StartCoroutine(KO(headButtingPlayer));
        }
        else
        {
            Knockback(headButtingPlayer);
            //ChangeFace(); TODO: replace

            Invoke("EnableBeingHit", graceTime / 2f);
        }
    }

    public void Kicked(GameObject headButtingPlayer)
    {
        Gameplay.Instance.PlayerKickedHandler.HandleKicked(Player.GetPlayerComponent(gameObject), Player.GetPlayerComponent(headButtingPlayer));

        PlayerHealth playerHealth = Player.GetPlayerComponent(gameObject).GetComponent<PlayerHealth>();
        playerHealth.ReduceHealth(kickingDamage);

        // TODO: handle KO elsewhere
        if (playerHealth.Health <= 0 && !IsKnockedOut())
        {
            StartCoroutine(KO(headButtingPlayer));
        }
        }
    //knocked out
    private IEnumerator KO(GameObject headButtingPlayer)
    {
        PlayerHealth playerHealth = Player.GetPlayerComponent(gameObject).GetComponent<PlayerHealth>();
        playerHealth.Recover();

        SetKnockedOut(true);
        //face.KnockedOut(); TODO: replace

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
        //face.Revived(); TODO: replace

        foreach (ConfigurableJoint bodyPart in bodyParts)
        {
            JointDrive springDriveX = bodyPart.angularXDrive;
            JointDrive springDriveYZ = bodyPart.angularYZDrive;

            springDriveX.positionSpring *= 100;
            springDriveYZ.positionSpring *= 100;

            bodyPart.angularXDrive = springDriveX;
            bodyPart.angularYZDrive = springDriveYZ;
        }

        StartCoroutine(ImproveEndurance());

        yield return new WaitForSeconds(graceTime);

        Invoke("EnableBeingHit", graceTime);
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
        PlayerHealth playerHealth = Player.GetPlayerComponent(gameObject).GetComponent<PlayerHealth>();
        playerHealth.IncreaseRecoveryHealth(2);
    }

    public bool CanBeHit()
    {
        return m_canBeHit;
    }

    private void EnableBeingHit()
    {
        m_canBeHit = true;
    }

    private void PreventFromBeingHit()
    {
        m_canBeHit = false;
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
