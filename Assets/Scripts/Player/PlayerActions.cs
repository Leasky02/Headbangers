using System.Collections;
using UnityEngine;

// TODO: move some logic into body class

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint cjBody;
    [SerializeField] private ConfigurableJoint cjLeftThigh;

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

    private Vector2 normalizedInputDirection;

    public void SetMoveDirection(Vector2 normalizedDirection)
    {
        normalizedInputDirection = normalizedDirection;
    }

    public void AttemptJump()
    {
        if (CanJump())
        {
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

    public void AttemptSit()
    {
        if (ShouldSit())
        {
            PlaySitAnimation();
        }
    }

    public void StopSitting()
    {
        isSitting = false;
    }

    public void AttemptKick()
    {
        if (ShouldKick())
        {
            StartCoroutine(Kick());
        }
    }

    public void AttemptHeadbutt()
    {
        if (ShouldHeadButt())
        {
            StartCoroutine(HeadButt());
        }
    }

    // Fixed Update is called 30x per frame
    void FixedUpdate()
    {
        if (CanWalk())
        {
            Vector3 moveDirection = new Vector3(normalizedInputDirection.x, 0f, normalizedInputDirection.y);
            isWalking = (moveDirection.sqrMagnitude > 0);
            if (isWalking)
            {
                Walk(moveDirection);
            }
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
                    Player.GetPlayerComponent(gameObject).GetComponent<PlayerAudio>().PlayAttackSound();
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

                Player.GetPlayerComponent(gameObject).GetComponent<PlayerAudio>().PlayAttackSound();

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
    private bool CanJump()
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
        if (!IsKnockedOut()) // TODO: is this required?
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
        Player.GetPlayerComponent(gameObject).GetComponent<PlayerBody>().Jump(jumpForce);
    }

    private IEnumerator Kick()
    {
        if (!IsKnockedOut()) // TODO: is this required?
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
        Vector3 walkingForce = direction * speed * Time.deltaTime;
        // TODO: confirm logic with Alasdair
        if (!IsDead() || (IsDead() && !groundDetector.IsGrounded()))
        {
            // Do nothing
        }
        else if (!IsDead() && !groundDetector.IsGrounded())
        {
            walkingForce *= 3;
        }
        else
        {
            walkingForce *= 2;
        }

        // Move the player
        Player.GetPlayerComponent(gameObject).GetComponent<PlayerBody>().AddForce(walkingForce);

        // Rotate the player
        Player.GetPlayerComponent(gameObject).GetComponent<PlayerBody>().RotateTowards(direction);

        PlayWalkAnimation();
    }

    // Animations

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

    public bool CanWalk()
    {
        return !isKicking && !isSitting && !isHeadbutting && !IsKnockedOut();
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
}
