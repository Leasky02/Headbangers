using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private Rigidbody rb;

    private ConfigurableJoint cj;
    [SerializeField] private ConfigurableJoint cjBody;
    [SerializeField] private ConfigurableJoint cjLeftThigh;

    private PlayerKO KOscript;

    [SerializeField] private AudioSource audioSource_Attack;

    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;
    private int jumpCount = 0;
    [SerializeField] private const int jumpLimit = 2;

    [SerializeField] private Animator decoyAnimator;

    [SerializeField] private GroundedDetector groundDetector;
    [SerializeField] private BodyDetector bodyDetector_HEAD;
    [SerializeField] private BodyDetector bodyDetector_FOOT;

    [SerializeField] private FacialEmotions face;

    private bool canHeadButt = true;
    private bool canKick = true;

    private bool canPlayHitSound = true;

    [HideInInspector] public bool attemptingHeadButt = false;
    private bool isWalking = false;
    private bool isSitting = false;
    private bool isKicking = false;

    [SerializeField] private float kickTimeLength;


    [SerializeField] private float HeadButtLength;
    [SerializeField] private float cooldownLength;

    [SerializeField] private float normalSpringValue_BODY;
    [SerializeField] private float normalSpringValue_LeftThigh;
    [SerializeField] private float attackSpringValue_BODY;
    [SerializeField] private float attackSpringValue_LeftThigh;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cj = GetComponent<ConfigurableJoint>();
        KOscript = GetComponent<PlayerKO>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get input direction and move
        float horizontalInput = Input.GetAxis("Horizontal" + playerData.GetPlayerID());
        float verticalInput = Input.GetAxis("Vertical" + playerData.GetPlayerID());

        //calculate movement direction of the player
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        direction = direction.normalized;

        //Walk
        isWalking = (Mathf.Abs(direction.magnitude) > 0.4f) && !isKicking && !isSitting && !attemptingHeadButt && !KOscript.IsKnockedOut();
        if (isWalking)
        {
            Walk(direction);
        }
    }

    private void Update()
    {
        //Kick
        bool shouldKick = Input.GetButtonDown("Kick" + playerData.GetPlayerID()) && ShouldKick();
        if (shouldKick)
        {
            StartCoroutine(Kick());
        }

        //HeadButt
        bool shouldHeadButt = Input.GetButtonDown("HeadButt" + playerData.GetPlayerID()) && ShouldHeadButt();
        if (shouldHeadButt)
        {
            StartCoroutine(HeadButt());
        }

        //Jump
        bool shouldJump = Input.GetButtonDown("Jump" + playerData.GetPlayerID()) && ShouldJump();
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
        isSitting = Input.GetButton("Sit" + playerData.GetPlayerID()) && ShouldSit();
        if (isSitting)
        {
            PlaySitAnimation();
        }

        //Idle
        bool shouldIdle = ShouldIdle();
        if (shouldIdle)
        {
            PlayIdleAnimation();
        }

        //successful HeadButt
        bool HeadButtingPlayer = attemptingHeadButt && bodyDetector_HEAD.IsTouchingBody() != null;
        if (HeadButtingPlayer)
        {
            if(canPlayHitSound)
            {
                audioSource_Attack.pitch = Random.Range(0.8f, 1.4f);
                audioSource_Attack.Play();
                canPlayHitSound = false;
            }

            PlayerKO victim = bodyDetector_HEAD.IsTouchingBody().transform.parent.gameObject.GetComponent<PlayerKO>();
            if (!victim.IsHeadButtInProgress())
            {
                victim.StartCoroutine(victim.HeadButted(gameObject , cjBody.gameObject.transform.localRotation.eulerAngles.x));

                StartCoroutine(face.ChangeEmotion("angry", "open", "happy", 3f));
            }
        }

        //successful kick
        bool kickingPlayer = isKicking && bodyDetector_FOOT.IsTouchingBody() != null;
        if(kickingPlayer)
        {
            isKicking = false;

            audioSource_Attack.pitch = Random.Range(0.8f, 1.4f);
            audioSource_Attack.Play();

            PlayerKO victim = bodyDetector_FOOT.IsTouchingBody().transform.parent.gameObject.GetComponent<PlayerKO>();
            victim.Kicked(gameObject);

            StartCoroutine(face.ChangeEmotion("angry", "open", "happy", 3f));
        }
    }
    private bool ShouldKick()
    {
        return canKick && !isSitting && !KOscript.IsKnockedOut();
    }
    private bool ShouldHeadButt()
    {
        return !isSitting && canHeadButt && !KOscript.IsKnockedOut();
    }
    private bool ShouldJump()
    {
        return !isKicking && !isSitting && !KOscript.IsKnockedOut();
    }
    private bool ShouldSit()
    {
        return !isKicking && !KOscript.IsKnockedOut();
    }
    private bool ShouldIdle()
    {
        return !isKicking && !isSitting && !isWalking && !attemptingHeadButt;
    }

    private IEnumerator HeadButt()
    {
        StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 1f));

        JointDrive springDriveX = cjBody.angularXDrive;
        JointDrive springDriveYZ = cjBody.angularYZDrive;

        springDriveX.positionSpring = attackSpringValue_BODY;
        springDriveYZ.positionSpring = attackSpringValue_BODY;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        canHeadButt = false;
        attemptingHeadButt = true;

        PlayHeadButtAnimation();

        yield return new WaitForSeconds(HeadButtLength/2);

        springDriveX.positionSpring = normalSpringValue_BODY;
        springDriveYZ.positionSpring = normalSpringValue_BODY;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        yield return new WaitForSeconds(HeadButtLength / 2);

        attemptingHeadButt = false;

        yield return new WaitForSeconds(cooldownLength);

        canHeadButt = true;
        canPlayHitSound = true;
    }

    private void Jump()
    {
        jumpCount++;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private IEnumerator Kick()
    {
        StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 1f));

        canKick = false;

        isKicking = true;
        PlayKickAnimation();

        JointDrive springDriveX = cjLeftThigh.angularXDrive;
        JointDrive springDriveYZ = cjLeftThigh.angularYZDrive;

        springDriveX.positionSpring = attackSpringValue_LeftThigh;
        springDriveYZ.positionSpring = attackSpringValue_LeftThigh;

        cjLeftThigh.angularXDrive = springDriveX;
        cjLeftThigh.angularYZDrive = springDriveYZ;

        yield return new WaitForSeconds(kickTimeLength / 2);

        isKicking = false;

        springDriveX.positionSpring = normalSpringValue_LeftThigh;
        springDriveYZ.positionSpring = normalSpringValue_LeftThigh;

        cjLeftThigh.angularXDrive = springDriveX;
        cjLeftThigh.angularYZDrive = springDriveYZ;

        yield return new WaitForSeconds(kickTimeLength / 2);

        canKick = true;
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

    private void PlayHeadButtAnimation()
    {
        decoyAnimator.Play("HeadButt");
    }

    private void PlayKickAnimation()
    {
        decoyAnimator.Play("Kick");
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

    public bool IsSitting()
    {
        return isSitting;
    }
    public bool IsKicking()
    {
        return isKicking;
    }

    public bool IsAttemptingHeadButt()
    {
        return attemptingHeadButt;
    }
}
