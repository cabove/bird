using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager Instance;

    public AudioSource audioSource;
    public float bpm = 120f;
    public float songOffset = 0f;

    private float secondsPerBeat;

    void Awake()
    {
        Instance = this;
        secondsPerBeat = 60f / bpm;
    }

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public float GetSongTime()
    {
        if (audioSource == null) return 0f;
        return audioSource.time - songOffset;
    }

    public float GetCurrentBeat()
    {
        return GetSongTime() / secondsPerBeat;
    }

    public float GetSecondsPerBeat()
    {
        return secondsPerBeat;
    }
}
