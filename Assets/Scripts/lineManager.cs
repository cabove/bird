using System.Collections;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public LineSegment linePrefab;

    public Vector3 currentLinePosition = new Vector3(-8f, 0.05f, 0f);
    public Vector3 previewLinePosition = new Vector3(-8f, -0.05f, 0f);

    public Transform player;
    public float playerStartX = -7f;
    public float playerEndX = 7f;
    public float playerY = 0f;

    public float sideSwitchDuration = 0.2f;

    private LineSegment currentLine;
    private LineSegment previewLine;

    private bool isSwitching = false;

    private birdscript bird;

    void Start()
    {
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
        if (currentLine == null) return;
        if (RhythmManager.Instance == null) return;

        MusicLineData currentData = currentLine.GetLineData();
        if (currentData == null) return;

        float currentBeat = RhythmManager.Instance.GetCurrentBeat();
        float beatInLine = currentBeat - currentData.startBeat;

        float t = Mathf.Clamp01(beatInLine / currentData.TotalBeats);

        UpdatePlayerPosition(t);

        if (beatInLine >= currentData.TotalBeats)
        {
            StartCoroutine(AdvanceLineAnimated());
        }
    }

    public void AdvanceLine()
    {
        if (!isSwitching)
        {
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

        MusicLineData currentData = currentLine.GetLineData();

        MusicLineData newPreviewData = RhythmGenerator.GenerateRandomLine();
        newPreviewData.startBeat = currentData.startBeat + currentData.TotalBeats;

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
        currentData.startBeat = 0f;

        MusicLineData previewData = RhythmGenerator.GenerateRandomLine();
        previewData.startBeat = currentData.startBeat + currentData.TotalBeats;

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

    public LineSegment GetCurrentLine()
    {
        return currentLine;
    }
}