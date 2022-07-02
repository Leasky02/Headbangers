using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedDetector : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private bool isGrounded = true;

    private bool springReachedMax = true;
    private bool springReachedMin = true;

    [SerializeField] private ConfigurableJoint[] bodyparts;

    //when collides with ground
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Interactable"))
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

    //when leaves the ground
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Interactable"))
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

    //adjusts body springs depending on isGrounded
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
