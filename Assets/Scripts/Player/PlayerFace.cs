using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    [SerializeField] private FacialEmotions face;

    public void UpdateFaceBasedOnHealth()
    {
        // TODO: facial emotions handler
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth.Health > 50)
        {
            StartCoroutine(face.ChangeEmotion("sad", "open", "sad", 2f));
        }
        else if (playerHealth.Health > 0)
        {
            StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 3f));
        }
    }

    public void AttackingFace()
    {
        StartCoroutine(face.ChangeEmotion("angry", "open", "happy", 3f));
    }

    public void AttemptingAttackFace()
    {
        // TODO: fix issue where face quickly changes from AttemptingAttackFace to AttackingFace
        StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 1f));
    }

    public void KnockedOut()
    {
        face.KnockedOut();
    }

    public void Revived()
    {
        face.Revived();
    }
}
