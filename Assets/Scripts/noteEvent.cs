public enum NoteType
{
    Quarter,
    EighthPair
}

[System.Serializable]
public class NoteEvent
{
    public float beatPosition;
    public NoteType noteType;
}