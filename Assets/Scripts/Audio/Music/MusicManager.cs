using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private float _targetVolume;
    [SerializeField] private float normalVolume = 0.5f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private float fadeSpeed = 0.02f;

    [SerializeField] private AudioClip mainTrack;
    [SerializeField] private AudioClip[] musicTracks;

    private int[] _shuffledSoundtrackIndicies;

    private int _trackIndex = 0;

    private bool _stopping = false;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        _targetVolume = normalVolume;

        _shuffledSoundtrackIndicies = new int[musicTracks.Length];

        ShuffleTracks();
        PlayMainSoundtrack();
    }

    private void ShuffleTracks()
    {
        ArrayHelpers.Shuffle(_shuffledSoundtrackIndicies);
    }

    public void PlayMainSoundtrack()
    {
        PlayTrack(mainTrack);
    }

    public void PlayNextTrack()
    {
        PlayTrack(musicTracks[_shuffledSoundtrackIndicies[_trackIndex]]);

        _trackIndex++;

        if (_trackIndex > _shuffledSoundtrackIndicies.Length)
        {
            _trackIndex = 0;
        }
    }

    public void PlayTrack(AudioClip track)
    {
        UnMute();
        audioSource.clip = track;
        audioSource.Play();
        _stopping = false;
    }

    public void StopTrack()
    {
        Mute();
        _stopping = true;
    }

    public void Mute()
    {
        _targetVolume = 0f;
    }

    public void UnMute()
    {
        _targetVolume = normalVolume;
    }

    private void Update()
    {
        float currentVolume = audioSource.volume;
        if (currentVolume != _targetVolume)
        {
            audioSource.volume = Mathf.Lerp(currentVolume, _targetVolume, fadeSpeed);
        }
        else if (_stopping)
        {
            audioSource.Stop();
            _stopping = false;
        }
    }
}