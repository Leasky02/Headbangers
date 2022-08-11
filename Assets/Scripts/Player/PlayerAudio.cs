using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource attack;
    [SerializeField] private AudioSource knockout;
    [SerializeField] private AudioSource interaction;
    [SerializeField] private AudioSource dead;

    public void PlayAttackSound()
    {
        attack.pitch = Random.Range(0.8f, 1.4f);
        attack.Play();
    }

    public void PlayKnockOutSound()
    {
        knockout.pitch = Random.Range(0.8f, 1.2f);
        knockout.Play();
    }

    public void PlayInteractionSound()
    {
        interaction.pitch = Random.Range(0.8f, 1.4f);
        interaction.Play();
    }

    public void PlayDeadSound()
    {
        dead.Play();
    }
}
