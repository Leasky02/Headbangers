using System.Collections;
using UnityEngine;

public class PlayerKick : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint cjLeftThigh;
    [SerializeField] private BodyDetector bodyDetector_FOOT;
    [SerializeField] private float kickingDamage = 10;
    [SerializeField] private float kickTimeLength = 0.4f;
    [SerializeField] private float normalSpringValue_LeftThigh = 1000;
    [SerializeField] private float attackSpringValue_LeftThigh = 3000;

    private bool canKick = true;
    private bool isKicking = false;

    private void Update()
    {
        if (isKicking)
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

    public void AttemptKick()
    {
        if (CanKick())
        {
            StartCoroutine(Kick());
        }
    }

    private IEnumerator Kick()
    {
        // StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 1f)); // TODO: uncomment

        canKick = false;

        isKicking = true;
        GetComponent<PlayerDecoyAnimator>().PlayKickAnimation();

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

    private bool CanKick()
    {
        return canKick && !GetComponent<PlayerSit>().IsSitting() && !GetComponent<Player>().IsKnockedOut() && !GetComponent<Player>().IsDead();
    }

    public bool IsKicking()
    {
        return isKicking;
    }

    public float GetKickingDamage()
    {
        return kickingDamage;
    }
}
