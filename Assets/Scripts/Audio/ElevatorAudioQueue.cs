using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAudioQueue : MonoBehaviour
{
    [SerializeField] private MapSelectionManager mapManager;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource dingAudioSource;
    [SerializeField] private AudioSource ambienceAudioSource;
    [SerializeField] private AudioSource doorAudioSource;

    public void PlayMusic()
    {
        musicAudioSource.Play();
    }
    public void PlayDing()
    {
        dingAudioSource.Play();
        RevealText();
    }
    public void PlayAmbience()
    {
        ambienceAudioSource.Play();
    }
    public void PlayDoor()
    {
        doorAudioSource.Play();
    }

    public void RevealText()
    {
        mapManager.RevealText();
    }
}
