using UnityEngine;

public class LineSegment : MonoBehaviour
{
    public Transform notesParent;
    public Transform barLinesParent;
    public GameObject quarterNotePrefab;
    public GameObject eighthPairPrefab;
    public Sprite barLineSprite;

    public float lineWidth = 16f;
    public float quarterNoteY = 0f;
    public float eighthPairY = 0.08f;
    public float quarterScale = 0.45f;
    public float eighthPairScale = 0.30f;
    public float barLineHeight = 0.8f;

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
        if (currentData == null || quarterNotePrefab == null || eighthPairPrefab == null) return;

        float totalBeats = currentData.TotalBeats;
        float leftX = -lineWidth / 2f;
        float rightX = lineWidth / 2f;

        float noteShift = 0.4f;
        foreach (NoteEvent note in currentData.notes)
        {
            float t = note.beatPosition / totalBeats;
            float x = Mathf.Lerp(leftX, rightX, t) + noteShift;

            if (note.noteType == NoteType.Quarter)
            {
                GameObject noteObj = Instantiate(quarterNotePrefab, notesParent);
                noteObj.transform.localPosition = new Vector3(x, quarterNoteY, 0f);
                noteObj.transform.localScale = new Vector3(quarterScale, quarterScale, 1f);
                SetSorting(noteObj);
            }
            else if (note.noteType == NoteType.EighthPair)
            {
                GameObject pairObj = Instantiate(eighthPairPrefab, notesParent);
                pairObj.transform.localPosition = new Vector3(x, eighthPairY, 0f);
                pairObj.transform.localScale = new Vector3(eighthPairScale, eighthPairScale, 1f);
                SetSorting(pairObj);
            }
        }
    }

    void SetSorting(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 2;
        }
    }

    void CreateBarLines()
    {
        if (currentData == null || barLineSprite == null) return;

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
            barLine.transform.localPosition = new Vector3(x, 0.45f, 0f);

            SpriteRenderer sr = barLine.AddComponent<SpriteRenderer>();
            sr.sprite = barLineSprite;
            sr.color = Color.black;
            sr.sortingOrder = 2;

            barLine.transform.localScale = new Vector3(0.03f, barLineHeight, 1f);
        }
    }

    void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
    public MusicLineData GetLineData() 
    {
        return currentData;
    }
}
