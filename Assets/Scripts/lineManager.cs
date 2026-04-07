using UnityEngine;

public class LineManager : MonoBehaviour
{
    public LineSegment linePrefab;

    public Vector3 currentLinePosition = new Vector3(-8f, 1f, 0f);
    public Vector3 previewLinePosition = new Vector3(-8f, -1.5f, 0f);

    private LineSegment currentLine;
    private LineSegment previewLine;

    void Start()
    {
        SpawnInitialLines();
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

    public void AdvanceLine()
    {
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }

        currentLine = previewLine;
        currentLine.transform.position = currentLinePosition;
        SetCurrentLook(currentLine);

        MusicLineData newPreviewData = RhythmGenerator.GenerateRandomLine();
        previewLine = Instantiate(linePrefab, previewLinePosition, Quaternion.identity);
        previewLine.BuildLine(newPreviewData);
        SetPreviewLook(previewLine);
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
}
