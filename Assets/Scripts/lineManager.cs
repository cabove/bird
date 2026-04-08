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

    public float bpm = 72f;
    public int beatsPerMeasure = 4;
    public int measuresPerLine = 4;

    private float lineDuration;
    private float lineTimer = 0f;

    private LineSegment currentLine;
    private LineSegment previewLine;

    void Start()
    {
        float secondsPerBeat = 60f / bpm;
        lineDuration = secondsPerBeat * beatsPerMeasure * measuresPerLine;

        SpawnInitialLines();

        if (player != null)
        {
            player.position = new Vector3(playerStartX, playerY, 0f);
        }
    }

    void Update()
    {
        lineTimer += Time.deltaTime;

        float t = Mathf.Clamp01(lineTimer / lineDuration);

        UpdatePlayerPosition(t);

        if (lineTimer >= lineDuration)
        {
            lineTimer = 0f;
            AdvanceLine();
        }
    }

    void UpdatePlayerPosition(float t)
    {
        if (player == null) return;

        float x = Mathf.Lerp(playerStartX, playerEndX, t);
        player.position = new Vector3(x, player.position.y, 0f);
    }

    void SpawnInitialLines()
    {
        MusicLineData currentData = RhythmGenerator.GenerateRandomLine();
        MusicLineData previewData = RhythmGenerator.GenerateRandomLine();

        currentLine = Instantiate(linePrefab, currentLinePosition, Quaternion.identity);
        currentLine.BuildLine(currentData);
        SetCurrentLook(currentLine);
        SetLineCollision(currentLine, true);

        previewLine = Instantiate(linePrefab, previewLinePosition, Quaternion.identity);
        previewLine.BuildLine(previewData);
        SetPreviewLook(previewLine);
        SetLineCollision(previewLine, false);
    }

    public void AdvanceLine()
    {
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }

        currentLine = previewLine;
        currentLine.transform.position = currentLinePosition;
        SetCurrentLook(currentLine);
        SetLineCollision(currentLine, true);

        MusicLineData newPreviewData = RhythmGenerator.GenerateRandomLine();
        previewLine = Instantiate(linePrefab, previewLinePosition, Quaternion.identity);
        previewLine.BuildLine(newPreviewData);
        SetPreviewLook(previewLine);
        SetLineCollision(previewLine, false);

        if (player != null)
        {
            player.position = new Vector3(playerStartX, playerY, 0f);

            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }
        }
    }

    void SetPreviewLook(LineSegment line)
    {
        SpriteRenderer[] renderers = line.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            c.a = 0.4f;
            sr.color = c;
        }
    }

    void SetCurrentLook(LineSegment line)
    {
        SpriteRenderer[] renderers = line.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
    }

    void SetLineCollision(LineSegment line, bool enabledState)
    {
        Collider2D[] colliders = line.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = enabledState;
        }
    }
}