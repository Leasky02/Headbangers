using System.Collections;
using UnityEngine;

public class PlayerHeadbutt : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint cjBody;
    [SerializeField] private Animator decoyAnimator;
    [SerializeField] private BodyDetector bodyDetector_HEAD;
    [SerializeField] private float HeadbuttLength = 0.15f;
    [SerializeField] private float coolDownLength = 0.25f;
    [SerializeField] private float normalSpringValue_BODY = 20;
    [SerializeField] private float attackSpringValue_BODY = 120;

    private bool canHeadbutt = true;
    private bool canPlayHitSound = true;
    private bool isHeadbutting = false;

    public void Update()
    {
        if (isHeadbutting)
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
    }

    public void AttemptHeadbutt()
    {
        if (CanHeadbutt())
        {
            StartCoroutine(Headbutt());
        }
    }

    private IEnumerator Headbutt()
    {
        // StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 1f)); // TODO: uncomment

        //change spring joints to be higher for faster head butt
        JointDrive springDriveX = cjBody.angularXDrive;
        JointDrive springDriveYZ = cjBody.angularYZDrive;

        springDriveX.positionSpring = attackSpringValue_BODY;
        springDriveYZ.positionSpring = attackSpringValue_BODY;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        canHeadbutt = false;
        isHeadbutting = true;

        PlayHeadbuttAnimation();

        yield return new WaitForSeconds(HeadbuttLength / 2);

        //reset spring joint back to normal
        springDriveX.positionSpring = normalSpringValue_BODY;
        springDriveYZ.positionSpring = normalSpringValue_BODY;

        cjBody.angularXDrive = springDriveX;
        cjBody.angularYZDrive = springDriveYZ;

        yield return new WaitForSeconds(HeadbuttLength / 2);

        isHeadbutting = false; // TODO: Ask Alasdair if this should go before yield i.e. on way back up from headbutt

        yield return new WaitForSeconds(coolDownLength);

        canHeadbutt = true;
        canPlayHitSound = true;
    }

    private void PlayHeadbuttAnimation()
    {
        decoyAnimator.speed = 1f;
        decoyAnimator.Play("Headbutt");
    }

    private bool CanHeadbutt()
    {
        return !GetComponent<PlayerSit>().IsSitting() && canHeadbutt && !GetComponent<Player>().IsKnockedOut();
    }

    public bool IsHeadbutting()
    {
        // TODO: Check with Alasdair - We probably need to distinguish between isHeadbutting and which will be set to false when the player starts to move back up
        // and whether a headbutt has made a connection or not yet.
        // We probably want to say the player take less damage if the headbutt motion is still downward whether or not the connection has been made yet
        // Can probably test with player debug health bars.
        return isHeadbutting;
    }
}
