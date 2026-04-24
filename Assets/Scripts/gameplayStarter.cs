using System.Collections;
using UnityEngine;

public class GameplayStarter : MonoBehaviour
{
    public Transform player;
    public Vector3 playerStartPosition = new Vector3(-7f, 0f, 0f);

    public Rigidbody2D playerRb;

    [Header("Countdown")]
    public AudioSource countdownAudioSource;
    public AudioClip hiHatClick;
    public float bpm = 72f;
    public int countInBeats = 4;

    private Coroutine countdownCoroutine;

    public void StartLevel()
    {
        Time.timeScale = 1f;

        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        if (player != null)
            player.position = playerStartPosition;

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        if (RhythmManager.Instance != null && RhythmManager.Instance.audioSource != null)
        {
            RhythmManager.Instance.audioSource.Stop();
            RhythmManager.Instance.audioSource.time = 0f;
        }

        countdownCoroutine = StartCoroutine(CountdownThenStartMusic());
    }

    IEnumerator CountdownThenStartMusic()
    {
        float secondsPerBeat = 60f / bpm;

        for (int i = 0; i < countInBeats; i++)
        {
            if (countdownAudioSource != null && hiHatClick != null)
            {
                countdownAudioSource.PlayOneShot(hiHatClick);
            }

            yield return new WaitForSeconds(secondsPerBeat);
        }

        if (RhythmManager.Instance != null)
        {
            RhythmManager.Instance.PlaySongFromBeginning();
        }
    }
}