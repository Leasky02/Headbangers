using UnityEngine;

public class PlayTrackOnLoad : MonoBehaviour
{
    [SerializeField]
    private AudioClip track;

    void Start()
    {
        MusicManager.Instance.PlayTrack(track);
    }
}
