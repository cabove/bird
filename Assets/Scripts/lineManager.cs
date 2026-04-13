using System.Collections;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public LineSegment linePrefab;

    public Vector3 currentLinePosition = new Vector3(0f, 0.75f, 0f);
    public Vector3 previewLinePosition = new Vector3(0f, -0.75f, 0f);

    public Transform player;
    public float playerStartX = -8f;
    public float playerEndX = 8f;
    public float playerY = 1.1f;

    public float sideSwitchDuration = 0.25f;

    public float bpm = 72f;
    public int beatsPerMeasure = 4;
    public int measuresPerLine = 4;

    public void AdvanceLine()
{
    StartCoroutine(AdvanceLineAnimated());
}

    private float lineDuration;
    private float lineTimer = 0f;

    private LineSegment currentLine;
    private LineSegment previewLine;

    private birdscript bird;
    private bool isSwitching = false;

    void Start()
    {
        float secondsPerBeat = 60f / bpm;
        lineDuration = secondsPerBeat * beatsPerMeasure * measuresPerLine;

        SpawnInitialLines();

        if (player != null)
        {
            player.position = new Vector3(playerStartX, playerY, player.position.z);
            bird = player.GetComponent<birdscript>();
        }
    }

    void Update()
    {
        if (isSwitching) return;

        lineTimer += Time.deltaTime;
        float t = Mathf.Clamp01(lineTimer / lineDuration);

        UpdatePlayerPosition(t);

        if (lineTimer >= lineDuration)
        {
            lineTimer = 0f;
            StartCoroutine(AdvanceLineAnimated());
        }
    }

    IEnumerator AdvanceLineAnimated()
    {
        isSwitching = true;

        if (bird != null)
            bird.allowAutoMove = false;

        Vector3 startPos = player.position;
        Vector3 endPos = new Vector3(playerStartX, player.position.y, startPos.z);

        float elapsed = 0f;

        while (elapsed < sideSwitchDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / sideSwitchDuration);
            player.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        player.position = endPos;

        if (currentLine != null)
            Destroy(currentLine.gameObject);

        currentLine = previewLine;
        currentLine.transform.position = currentLinePosition;
        SetCurrentLook(currentLine);

        MusicLineData newPreviewData = RhythmGenerator.GenerateRandomLine();
        previewLine = Instantiate(linePrefab, previewLinePosition, Quaternion.identity);
        previewLine.BuildLine(newPreviewData);
        SetPreviewLook(previewLine);

        if (bird != null)
            bird.allowAutoMove = true;

        isSwitching = false;
    }

    void SpawnInitialLines()
    {
        MusicLineData currentData = RhythmGenerator.GenerateRandomLine();
        MusicLineData previewData = RhythmGenerator.GenerateRandomLine();

        currentLine = Instantiate(linePrefab, currentLinePosition, Quaternion.identity);
        currentLine.BuildLine(currentData);

        previewLine = Instantiate(linePrefab, previewLinePosition, Quaternion.identity);
        previewLine.BuildLine(previewData);

        SetPreviewLook(previewLine);
    }

    void UpdatePlayerPosition(float t)
    {
        if (player == null) return;

        float x = Mathf.Lerp(playerStartX, playerEndX, t);
        player.position = new Vector3(x, player.position.y, player.position.z);
    }

    void SetPreviewLook(LineSegment line) { }
    void SetCurrentLook(LineSegment line) { }
}