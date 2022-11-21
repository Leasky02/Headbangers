using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private float targetVolume;
    [SerializeField] private float normalVolume = 0.5f;
    private AudioSource myAudioSource;
    [SerializeField] private float fadeSpeed = 0.1f;

    [SerializeField] private AudioClip mainSoundtrack;
    [SerializeField] private AudioClip[] soundtracks;

    //default list
    private List<AudioClip> soundtrackList;
    //shuffled list
    private List<AudioClip> shuffledSoundtrackList;

    private int trackIndex = 0;


    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        targetVolume = normalVolume;

        InitialiseDefaultList();
        ShuffleTracks();
        //play main theme tune
        PlayMainSoundtrack();
    }

    public void InitialiseDefaultList()
    {
        //create list of tracks
        soundtrackList = new List<AudioClip>();
        //set each track into list
        foreach (AudioClip track in soundtracks)
        {
            soundtrackList.Add(track);
        }
    }

    public void ShuffleTracks()
    {
        shuffledSoundtrackList = new List<AudioClip>();

        for(int i = 0; i < soundtracks.Length; i++)
        {
            int randomIndex = Random.Range(0, soundtrackList.Count);

            shuffledSoundtrackList.Add(soundtrackList[randomIndex]);
            soundtrackList.RemoveAt(randomIndex);

            Debug.Log(shuffledSoundtrackList[i]);
        }
    }

    public void PlayMainSoundtrack()
    {
        //return volume to normal
        UnMute();

        myAudioSource.clip = mainSoundtrack;
    }

    public void PlayNextTrack()
    {
        //return volume to normal
        UnMute();

        Debug.Log("track played");

        myAudioSource.clip = shuffledSoundtrackList[trackIndex];
        myAudioSource.Play();

        trackIndex++;
        //if track index is more than number of tracks (reached the end of the playlist)
        if(trackIndex > shuffledSoundtrackList.Count)
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
        Debug.Log("track stopped");
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