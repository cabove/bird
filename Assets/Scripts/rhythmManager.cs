using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public float maxSongLength = 35f;
    public static RhythmManager Instance;

    public AudioSource audioSource;
    public float bpm = 72f;
    public float songOffset = 0f;

    [Header("Results Panel")]
    public bool autoShowResultsPanelOnSongEnd = true;
    public GameObject resultsPanel;
    public GameObject[] gameplayObjectsToDisableOnSongEnd;

    private float secondsPerBeat;
    private bool hasTriggeredSongEnd = false;
    private bool songStarted = false;

    void Awake()
    {
        Instance = this;
        secondsPerBeat = 60f / bpm;
    }

    void Start()
    {
        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.time = 0f;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (!songStarted) return;
        if (!autoShowResultsPanelOnSongEnd || hasTriggeredSongEnd) return;
        if (audioSource == null || audioSource.clip == null) return;

        if ((!audioSource.isPlaying && audioSource.time >= audioSource.clip.length) || audioSource.time >= maxSongLength)
        {
            hasTriggeredSongEnd = true;
            audioSource.Stop();
            if (gameplayObjectsToDisableOnSongEnd != null)
            {
                foreach (GameObject go in gameplayObjectsToDisableOnSongEnd)
                {
                    if (go != null) go.SetActive(false);
                }
            }

            if (resultsPanel != null)
                resultsPanel.SetActive(true);
        }
    }

    public void PlaySongFromBeginning()
    {
        if (audioSource == null) return;

        hasTriggeredSongEnd = false;
        songStarted = true;

        audioSource.Stop();
        audioSource.time = 0f;
        audioSource.Play();
    }

    public bool HasSongStarted()
    {
        return songStarted;
    }

    public float GetSongTime()
    {
        if (!songStarted || audioSource == null) return 0f;
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