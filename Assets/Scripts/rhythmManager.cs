using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager Instance;

    public AudioSource audioSource;
    public float bpm = 72f;
    public float songOffset = 0f;

    [Header("Results Panel (Same Scene)")]
    public bool autoShowResultsPanelOnSongEnd = true;
    public GameObject resultsPanel;
    public GameObject[] gameplayObjectsToDisableOnSongEnd;

    private float secondsPerBeat;
    private bool hasTriggeredSongEnd = false;

    void Awake()
    {
        Instance = this;
        secondsPerBeat = 60f / bpm;
    }

    void Start()
    {
        if (resultsPanel != null) resultsPanel.SetActive(false);
        if (audioSource != null) audioSource.Play();
    }

    void Update()
    {
        if (!autoShowResultsPanelOnSongEnd || hasTriggeredSongEnd) return;
        if (audioSource == null || audioSource.clip == null) return;

        if (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length)
        {
            hasTriggeredSongEnd = true;

            if (gameplayObjectsToDisableOnSongEnd != null)
            {
                foreach (GameObject go in gameplayObjectsToDisableOnSongEnd)
                {
                    if (go != null) go.SetActive(false);
                }
            }

            if (resultsPanel != null) resultsPanel.SetActive(true);
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