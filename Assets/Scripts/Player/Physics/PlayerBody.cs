using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ConfigurableJoint cjBody;
    [SerializeField] private float knockBackForce = 20;
    [SerializeField] private float knockBackHeightVelocity = 10;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        rb.transform.position = position;
    }

    public void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnfreezePosition()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    public void KnockBack(Vector3 knockedBackFrom, bool knockBackAtHead = false)
    {
        Vector3 selfPosition = transform.position;
        Vector3 direction = new Vector3(selfPosition.x - knockedBackFrom.x, 0f, selfPosition.z - knockedBackFrom.z);
        direction = direction.normalized;

        if (knockBackAtHead)
        {
            Vector3 forcePosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            rb.AddForceAtPosition(direction * knockBackForce * 0.4f, forcePosition, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(direction * knockBackForce, ForceMode.Impulse);
        }

        direction.y = knockBackHeightVelocity;
        rb.velocity = new Vector3(direction.x, direction.y, direction.z);
    }

    public void KnockOut(GameObject knockedOutBy = null)
    {
        foreach (ConfigurableJoint bodyPart in GetBodyParts())
        {
            JointDrive springDriveX = bodyPart.angularXDrive;
            JointDrive springDriveYZ = bodyPart.angularYZDrive;

            springDriveX.positionSpring /= 100;
            springDriveYZ.positionSpring /= 100;

            bodyPart.angularXDrive = springDriveX;
            bodyPart.angularYZDrive = springDriveYZ;
        }
    }

    public void Revive()
    {
        foreach (ConfigurableJoint bodyPart in GetBodyParts())
        {
            JointDrive springDriveX = bodyPart.angularXDrive;
            JointDrive springDriveYZ = bodyPart.angularYZDrive;

            springDriveX.positionSpring *= 100;
            springDriveYZ.positionSpring *= 100;

            bodyPart.angularXDrive = springDriveX;
            bodyPart.angularYZDrive = springDriveYZ;
        }
    }

    private ConfigurableJoint[] GetBodyParts()
    {
        return rb.gameObject.GetComponentsInChildren<ConfigurableJoint>();
    }

    public float GetBodyAngle()
    {
        return cjBody.gameObject.transform.localRotation.eulerAngles.x;
    }
}
