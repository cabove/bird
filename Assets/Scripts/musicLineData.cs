using System.Collections.Generic;

[System.Serializable]
public class MusicLineData
{
    public int measureCount = 4;
    public int beatsPerMeasure = 4;
    public List<NoteEvent> notes = new List<NoteEvent>();

    public float TotalBeats
    {
        get { return measureCount * beatsPerMeasure; }
    }
}
