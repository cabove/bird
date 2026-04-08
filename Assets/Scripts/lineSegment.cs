using UnityEngine;

public class LineSegment : MonoBehaviour
{
    public Transform notesParent;
    public Transform barLinesParent;
    public GameObject notePrefab;

    public float lineWidth = 16f;
    public float noteY = 0.8f;
    public float barLineHeight = 1.5f;

    private MusicLineData currentData;

    public void BuildLine(MusicLineData lineData)
    {
        currentData = lineData;

        ClearChildren(notesParent);
        ClearChildren(barLinesParent);

        CreateBarLines();
        CreateNotes();
    }

    void CreateNotes()
    {
        if (currentData == null || notePrefab == null) return;

        float totalBeats = currentData.TotalBeats;
        float leftX = -lineWidth / 2f;
        float rightX = lineWidth / 2f;

        foreach (NoteEvent note in currentData.notes)
        {
            float t = note.beatPosition / totalBeats;
            float x = Mathf.Lerp(leftX, rightX, t);

            GameObject noteObj = Instantiate(notePrefab, notesParent);
            noteObj.transform.localPosition = new Vector3(x, noteY, 0f);
            noteObj.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
        }
    }

    void CreateBarLines()
    {
        if (currentData == null) return;

        float totalBeats = currentData.TotalBeats;
        float leftX = -lineWidth / 2f;
        float rightX = lineWidth / 2f;

        for (int measure = 1; measure < currentData.measureCount; measure++)
        {
            float beatPosition = measure * currentData.beatsPerMeasure;
            float t = beatPosition / totalBeats;
            float x = Mathf.Lerp(leftX, rightX, t);

            GameObject barLine = new GameObject("BarLine");
            barLine.transform.SetParent(barLinesParent);
            barLine.transform.localPosition = new Vector3(x, noteY, 0f);

            SpriteRenderer sr = barLine.AddComponent<SpriteRenderer>();
            sr.sprite = notePrefab.GetComponent<SpriteRenderer>().sprite;
            sr.color = Color.black;

            barLine.transform.localScale = new Vector3(0.05f, barLineHeight, 1f);
        }
    }

    void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
