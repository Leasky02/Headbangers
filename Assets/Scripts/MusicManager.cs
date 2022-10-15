using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private float targetVolume = 0.5f;
    private AudioSource myAudioSource;
    [SerializeField] private float fadeSpeed = 0.05f;

    [SerializeField] private AudioClip[] songs;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        PlaySetSong(0);
    }

    public void SetMusicVolume(float newVolume)
    {
        targetVolume = newVolume;
    }

    public void PlayRandomSong()
    {
        myAudioSource.clip = songs[Random.Range(0, songs.Length)];
        myAudioSource.Play();
    }
    public void PlaySetSong(int songToPlay)
    {
        myAudioSource.clip = songs[songToPlay];
        myAudioSource.Play();
    }

    private void Update()
    {
        float currentVolume = myAudioSource.volume;
        myAudioSource.volume = Mathf.Lerp(currentVolume, targetVolume, fadeSpeed);
    }
}