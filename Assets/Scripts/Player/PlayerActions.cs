using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private Rigidbody rb;
    private ConfigurableJoint cj;
    [SerializeField] private ConfigurableJoint cjBody;
    [SerializeField] private ConfigurableJoint cjLeftThigh;

    [SerializeField] private AudioSource audioSource_Attack;

    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;
    private int jumpCount = 0;
    [SerializeField] private const int jumpLimit = 1;

    [SerializeField] private Animator decoyAnimator;

    [SerializeField] private GroundedDetector groundDetector;
    [SerializeField] private BodyDetector bodyDetector_HEAD;
    [SerializeField] private BodyDetector bodyDetector_FOOT;

    [SerializeField] private FacialEmotions face;

    private bool canHeadButt = true;
    private bool canKick = true;

    private bool canPlayHitSound = true;

    private bool isHeadbutting = false;
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

    private Vector2 inputDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cj = GetComponent<ConfigurableJoint>();
    }

    //functions called from input
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        //only jump on performed
        if (!context.performed)
        {
            return;
        }

        if (ShouldJump())
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
    }
    public void OnSit(InputAction.CallbackContext context)
    {
        //only jump on performed
        if (context.performed)
        {
            isSitting = ShouldSit();
            if (isSitting)
            {
                PlaySitAnimation();
            }
        }
        else if (context.canceled)
        {
            isSitting = false;
        }

    }
    public void OnKick(InputAction.CallbackContext context)
    {
        //only jump on performed
        if (!context.performed)
        {
            return;
        }

        if (ShouldKick())
        {
            StartCoroutine(Kick());
        }
    }
    public void OnHeadButt(InputAction.CallbackContext context)
    {
        //only jump on performed
        if (!context.performed)
        {
            return;
        }

        if (ShouldHeadButt())
        {
            StartCoroutine(HeadButt());
        }
    }

    // Fixed Update is called 30x per frame
    void FixedUpdate()
    {
        inputDirection = inputDirection.normalized;

        Vector3 moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);
        //Walk
        isWalking = IsWalking(moveDirection);
        if (isWalking)
        {
            Walk(moveDirection);
        }
    }

    private void Update()
    {
        // Idle
        if (ShouldIdle())
        {
            PlayIdleAnimation();
        }

        // Headbutting
        if (IsHeadbutting())
        {
            GameObject bodyHeadbutted = bodyDetector_HEAD.IsTouchingBody();
            if (bodyHeadbutted != null)
            {
                if (canPlayHitSound) // TODO: is the required?
                {
                    audioSource_Attack.pitch = Random.Range(0.8f, 1.4f);
                    audioSource_Attack.Play();
                    canPlayHitSound = false;
                }

                Player playerHeadbutted = Player.GetPlayerComponent(bodyHeadbutted);
                Gameplay.Instance.PlayerHeadbuttedHandler.HandleHeadbutted(playerHeadbutted, Player.GetPlayerComponent(gameObject));
            }
        }

        // Kicking
        if (IsKicking())
        {
            GameObject bodyKicked = bodyDetector_FOOT.IsTouchingBody();
            if (bodyKicked != null)
            {
                isKicking = false;

                audioSource_Attack.pitch = Random.Range(0.8f, 1.4f);
                audioSource_Attack.Play();

                Player playerKicked = Player.GetPlayerComponent(bodyKicked);
                Gameplay.Instance.PlayerKickedHandler.HandleKicked(playerKicked, Player.GetPlayerComponent(gameObject));
            }
        }
    }

    private bool IsKnockedOut()
    {
        return Player.GetPlayerComponent(gameObject).IsKnockedOut();
    }

    private bool IsDead()
    {
        return Player.GetPlayerComponent(gameObject).IsDead();
    }

    //action checks
    private bool ShouldKick()
    {
        return canKick && !isSitting && !IsKnockedOut() && !IsDead();
    }
    private bool ShouldHeadButt()
    {
        return !isSitting && canHeadButt && !IsKnockedOut();
    }
    private bool ShouldJump()
    {
        return !isKicking && !isSitting && !IsKnockedOut();
    }
    private bool ShouldSit()
    {
        return !isKicking && !IsKnockedOut();
    }
    private bool ShouldIdle()
    {
        return !isKicking && !isSitting && !isWalking && !isHeadbutting;
    }

    private IEnumerator HeadButt()
    {
        if (!IsKnockedOut())
        {
            StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 1f));
        }

        //change spring joints to be higher for faster head butt
        JointDrive springDriveX = cjBody.angularXDrive;
        JointDrive springDriveYZ = cjBody.angularYZDrive;

        springDriveX.positionSpring = attackSpringValue_BODY;
        springDriveYZ.positionSpring = attackSpringValue_BODY;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        canHeadButt = false;
        isHeadbutting = true;

        PlayHeadButtAnimation();

        yield return new WaitForSeconds(HeadButtLength / 2);

        //reset spring joint back to normal
        springDriveX.positionSpring = normalSpringValue_BODY;
        springDriveYZ.positionSpring = normalSpringValue_BODY;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        yield return new WaitForSeconds(HeadButtLength / 2);

        isHeadbutting = false; // TODO: Ask Alasdair if this should go before yield i.e. on way back up from headbut

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
        if (!IsKnockedOut())
        {
            StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 1f));
        }

        canKick = false;

        isKicking = true;
        PlayKickAnimation();

        //increase joint spring for fast attack
        JointDrive springDriveX = cjLeftThigh.angularXDrive;
        JointDrive springDriveYZ = cjLeftThigh.angularYZDrive;

        springDriveX.positionSpring = attackSpringValue_LeftThigh;
        springDriveYZ.positionSpring = attackSpringValue_LeftThigh;

        cjLeftThigh.angularXDrive = springDriveX;
        cjLeftThigh.angularYZDrive = springDriveYZ;

        yield return new WaitForSeconds(kickTimeLength / 2);

        isKicking = false;

        //return joint spring back to normal
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
        if (!IsDead() || (IsDead() && !groundDetector.IsGrounded()))
        {
            rb.AddForce(direction * speed * Time.deltaTime);
        }
        else if (!IsDead() && !groundDetector.IsGrounded())
        {
            rb.AddForce(direction * speed * 3 * Time.deltaTime);
        }
        else
        {
            rb.AddForce(direction * speed * 2f * Time.deltaTime);
        }

        //rotate the player
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(-direction.x, direction.y, direction.z), Vector3.up);
        cj.targetRotation = toRotation;

        PlayWalkAnimation();
    }

    //animations
    private void PlayHeadButtAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("HeadButt");
    }

    private void PlayKickAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("Kick");
    }

    private void PlaySitAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("Sit");
    }

    private void PlayIdleAnimation()
    {
        decoyAnimator.Play("Idle");
    }
    private void PlayWalkAnimation()
    {
        if (!IsDead())
        {
            decoyAnimator.Play("Walk");
        }

        if (groundDetector.IsGrounded())
            decoyAnimator.speed = 1f;
        else
            decoyAnimator.speed = 0.5f;
    }

    //return checks for action states
    public bool IsWalking(Vector3 direction)
    {
        return (Mathf.Abs(direction.magnitude) > 0.4f) && !isKicking && !isSitting && !isHeadbutting && !IsKnockedOut();
    }

    public bool IsSitting()
    {
        return isSitting;
    }
    public bool IsKicking()
    {
        return isKicking;
    }

    public bool IsHeadbutting()
    {
        // TODO: Check with Alasdair - We probably need to distinguish between isHeadbutting and which will be set to false when the player starts to move back up
        // and whether a headbut has made a connection or not yet.
        // We probably want to say the player take less damage if the headbutt motion is still downward whether or not the connection has been made yet
        // Can probably test with player debug health bars.
        return isHeadbutting;
    }

    // TEMP

    public float GetBodyAngle()
    {
        return cjBody.gameObject.transform.localRotation.eulerAngles.x;
    }
}
