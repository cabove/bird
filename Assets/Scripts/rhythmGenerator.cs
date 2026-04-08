using UnityEngine;

public static class RhythmGenerator
{
    public static MusicLineData GenerateRandomLine()
    {
        MusicLineData line = new MusicLineData();

        int totalBeats = line.measureCount * line.beatsPerMeasure;

        for (int i = 0; i < totalBeats; i++)
        {
            NoteEvent note = new NoteEvent();
            note.beatPosition = i;

            // 50/50 chance (you can tweak this later)
            if (Random.value < 0.5f)
                note.noteType = NoteType.Quarter;
            else
                note.noteType = NoteType.EighthPair;

            line.notes.Add(note);
        }

        return line;
    }
}