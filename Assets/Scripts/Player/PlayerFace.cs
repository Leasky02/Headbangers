using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    [SerializeField] private FacialEmotions face;

    public void ChangeFace()
    {
        // TODO: facial emotions handler
        PlayerHealth playerHealth = Player.GetPlayerComponent(gameObject).GetComponent<PlayerHealth>();
        if (Player.GetPlayerComponent(gameObject).IsKnockedOut())
        {
            if (playerHealth.Health > 50)
            {
                StartCoroutine(face.ChangeEmotion("sad", "open", "sad", 2f));
            }
            else if (playerHealth.Health > 0)
            {
                StartCoroutine(face.ChangeEmotion("angry", "open", "sad", 3f));
            }
        }
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
