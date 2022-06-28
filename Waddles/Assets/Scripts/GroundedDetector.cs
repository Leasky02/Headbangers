using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedDetector : MonoBehaviour
{
    private bool isGrounded = true;

    [SerializeField] private bool springReachedMax = true;
    [SerializeField] private bool springReachedMin = true;

    [SerializeField] private ConfigurableJoint[] bodyparts;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;

            if(!springReachedMax)
            {
                AdjustSpring(2f);
                springReachedMax = true;
                springReachedMin = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;

            if(!springReachedMin)
            {
                AdjustSpring(0.5f);
                springReachedMax = false;
                springReachedMin = true;
            }
        }
    }

    private void AdjustSpring(float multiplier)
    {
        foreach(ConfigurableJoint bodyPart in bodyparts)
        {
            JointDrive springDriveX = bodyPart.angularXDrive;
            JointDrive springDriveYZ = bodyPart.angularYZDrive;

            springDriveX.positionSpring *= multiplier;
            springDriveYZ.positionSpring *= multiplier;

            springDriveX.positionDamper *= multiplier;
            springDriveYZ.positionDamper *= multiplier;

            bodyPart.angularXDrive = springDriveX;
            bodyPart.angularYZDrive = springDriveYZ;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
