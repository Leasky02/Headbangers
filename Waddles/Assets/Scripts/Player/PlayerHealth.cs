using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerActions actionScript;
    private ConfigurableJoint cj;
    private Rigidbody rb;

    private bool attackInProgress = false;
    private bool knockedOut = false;

    [SerializeField] private float deadSpringValue;
    [SerializeField] private float knockoutTime;
    [SerializeField] private float graceTime;

    [SerializeField] private float rotationForce;
    [SerializeField] private float knockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
        rb = GetComponent<Rigidbody>();
        actionScript = GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Attacked(GameObject attackingPlayer)
    {
        if (!actionScript.IsAttemptingAttack())
        {
            knockedOut = true;
            attackInProgress = true;

            Vector3 attackerPosition = attackingPlayer.transform.position;
            Vector3 selfPosition = transform.position;
            Vector3 direction = new Vector3(attackerPosition.x - selfPosition.x, 0f, attackerPosition.z - selfPosition.z);
            direction.Normalize();

            Vector3 forcePosition = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
            rb.velocity = new Vector3(-direction.x * knockbackForce, rb.velocity.y, -direction.y * knockbackForce);
            rb.AddForceAtPosition(-direction * rotationForce * Time.deltaTime, forcePosition, ForceMode.Impulse);

            ConfigurableJoint[] bodyParts = gameObject.GetComponentsInChildren<ConfigurableJoint>();
            foreach(ConfigurableJoint bodyPart in bodyParts)
            {
                JointDrive springDriveX = bodyPart.angularXDrive;
                JointDrive springDriveYZ = bodyPart.angularYZDrive;

                springDriveX.positionSpring /= 100;
                springDriveYZ.positionSpring /= 100;

                bodyPart.angularXDrive = springDriveX;
                bodyPart.angularYZDrive = springDriveYZ;
            }

            yield return new WaitForSeconds(knockoutTime);

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

            yield return new WaitForSeconds(graceTime);
            attackInProgress = false;

        }
        else
        {
            //both players were attacking
            yield break;
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
