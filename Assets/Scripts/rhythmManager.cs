using UnityEngine;
using UnityEngine.SceneManagement;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager Instance;

    public AudioSource audioSource;
    public float bpm = 120f;
    public float songOffset = 0f;
    [Header("Scene Transition")]
    public string resultsSceneName = "Results";
    public bool autoLoadResultsOnSongEnd = true;

    private float secondsPerBeat;
    private bool hasTriggeredSongEnd = false;

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

    void Update()
    {
        if (!autoLoadResultsOnSongEnd || hasTriggeredSongEnd) return;
        if (audioSource == null || audioSource.clip == null) return;

        if (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length)
        {
            hasTriggeredSongEnd = true;
            SceneManager.LoadScene(resultsSceneName);
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
