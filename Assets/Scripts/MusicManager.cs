using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private float targetVolume;
    [SerializeField] private float normalVolume = 0.5f;
    private AudioSource myAudioSource;
    [SerializeField] private float fadeSpeed = 0.02f;

    [SerializeField] private AudioClip mainSoundtrack;
    [SerializeField] private AudioClip[] soundtracks;

    private int[] shuffledSoundtrackIndicies;

    private int trackIndex = 0;


    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        targetVolume = normalVolume;

        shuffledSoundtrackIndicies = new int[soundtracks.Length];

        ShuffleTracks();
        PlayMainSoundtrack();
    }

    private void ShuffleTracks()
    {
        ArrayHelpers.Shuffle(shuffledSoundtrackIndicies);
    }

    public void PlayMainSoundtrack()
    {
        UnMute();

        myAudioSource.clip = mainSoundtrack;
    }

    public void PlayNextTrack()
    {
        UnMute();

        myAudioSource.clip = soundtracks[shuffledSoundtrackIndicies[trackIndex]];
        myAudioSource.Play();

        trackIndex++;
        
        if(trackIndex > shuffledSoundtrackIndicies.Length)
        {
            trackIndex = 0;
        }

    }

    public void CallStopTrack()
    {
        StartCoroutine(StopTrack());
    }

    public IEnumerator StopTrack()
    {
        Mute();
        yield return new WaitForSeconds(1f);
        myAudioSource.Stop();
    }

    public void Mute()
    {
        targetVolume = 0f;
    }

    public void UnMute()
    {
        targetVolume = normalVolume;
    }

    private void Update()
    {
        float currentVolume = myAudioSource.volume;
        myAudioSource.volume = Mathf.Lerp(currentVolume, targetVolume, fadeSpeed);
    }
}