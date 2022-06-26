using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private Rigidbody rb;
    private ConfigurableJoint cj;
    private PlayerKO ph;
    [SerializeField] private ConfigurableJoint cjBody;
    private AudioSource myAudioSource;

    [SerializeField] private AudioClip attack_CLIP;

    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;
    private int jumpCount = 0;
    [SerializeField] private const int jumpLimit = 2;

    [SerializeField] private Animator decoyAnimator;

    [SerializeField] private GroundedDetector groundDetector;
    [SerializeField] private BodyDetector bodyDetector;

    private bool isWalking = false;
    [HideInInspector] public bool attemptingAttack = false;
    private bool canAttack = true;
    private bool isSitting = false;

    private bool canPlayHitSound = true;

    [SerializeField] private float attackLength;
    [SerializeField] private float cooldownLength;

    [SerializeField] private float normalSpringValue;
    [SerializeField] private float attackSpringValue;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cj = GetComponent<ConfigurableJoint>();
        ph = GetComponent<PlayerKO>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get input direction and move
        float horizontalInput = Input.GetAxis("Horizontal" + playerData.GetPlayerID());
        float verticalInput = Input.GetAxis("Vertical" + playerData.GetPlayerID());

        //calculate movement direction of the player
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        direction.Normalize();

        //Walk
        isWalking = (Mathf.Abs(direction.magnitude) > 0.4f) && !isSitting && !attemptingAttack && !ph.IsKnockedOut();
        if (isWalking)
        {
            Walk(direction);
        }
    }

    private void Update()
    {
        //Attack
        bool shouldAttack = Input.GetButtonDown("Attack" + playerData.GetPlayerID()) && !isSitting && canAttack && !ph.IsKnockedOut();
        if (shouldAttack)
        {
            StartCoroutine(Attack());
        }

        //Jump
        bool shouldJump = Input.GetButtonDown("Jump" + playerData.GetPlayerID()) && !isSitting && !ph.IsKnockedOut();
        if (shouldJump)
        {
            //if on ground
            if (groundDetector.IsGrounded())
            {
                jumpCount = 0;
            }

            if (groundDetector.IsGrounded() || (jumpCount > 0 && jumpCount < jumpLimit))
            {
                Jump();
            }
        }

        //Sit
        isSitting = Input.GetButton("Sit" + playerData.GetPlayerID()) && !ph.IsKnockedOut();
        if (isSitting)
        {
            PlaySitAnimation();
        }

        //Idle
        if (IsIdle())
        {
            PlayIdleAnimation();
        }

        bool attackingPlayer = attemptingAttack && bodyDetector.IsTouchingBody() != null ;
        if (attackingPlayer)
        {
            if(canPlayHitSound)
            {
                myAudioSource.clip = attack_CLIP;
                myAudioSource.pitch = Random.Range(0.8f, 1.2f);
                myAudioSource.Play();
                canPlayHitSound = false;
            }

            PlayerKO victim = bodyDetector.IsTouchingBody().transform.parent.gameObject.GetComponent<PlayerKO>();
            if (!victim.IsAttackInProgress())
            {
                victim.StartCoroutine(victim.Attacked(gameObject , cjBody.gameObject.transform.localRotation.eulerAngles.x));
            }
        }
    }

    private IEnumerator Attack()
    {
        JointDrive springDriveX = cjBody.angularXDrive;
        JointDrive springDriveYZ = cjBody.angularYZDrive;

        springDriveX.positionSpring = attackSpringValue;
        springDriveYZ.positionSpring = attackSpringValue;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        canAttack = false;
        attemptingAttack = true;

        PlayAttackAnimation();

        yield return new WaitForSeconds(attackLength/2);

        springDriveX.positionSpring = normalSpringValue;
        springDriveYZ.positionSpring = normalSpringValue;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        yield return new WaitForSeconds(attackLength / 2);

        attemptingAttack = false;

        yield return new WaitForSeconds(cooldownLength);

        canAttack = true;
        canPlayHitSound = true;
    }

    private void PlayAttackAnimation()
    {
        decoyAnimator.Play("Attack");
    }

    private void Jump()
    {
        jumpCount++;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void PlaySitAnimation()
    {
        decoyAnimator.Play("Sit");
    }

    private void PlayIdleAnimation()
    {
        decoyAnimator.Play("Idle");
    }
    private void PlayWalkAnimation()
    {
        decoyAnimator.Play("Walk");
    }


    private void Walk(Vector3 direction)
    {
        //move the player
        rb.AddForce(direction * speed * Time.deltaTime);
        //rotate the player
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(-direction.x, direction.y, direction.z), Vector3.up);
        cj.targetRotation = toRotation;

        PlayWalkAnimation();
    }

    public bool IsSitting()
    {
        return isSitting;
    }

    private bool IsIdle()
    {
        return !isSitting && !isWalking && !attemptingAttack;
    }

    public bool IsAttemptingAttack()
    {
        return attemptingAttack;
    }
}
