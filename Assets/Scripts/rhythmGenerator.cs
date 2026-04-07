using System.Collections.Generic;
using UnityEngine;

public static class RhythmGenerator
{
    private static List<float[]> measurePatterns = new List<float[]>
    {
        new float[] { 0f, 1f, 2f, 3f },
        new float[] { 0f, 2f, 3f },
        new float[] { 0f, 1f, 2.5f },
        new float[] { 0f, 1.5f, 2f, 3.5f },
        new float[] { 0f, 0.5f, 1.5f, 2.5f },
        new float[] { 0f, 2f },
        new float[] { 0f, 1f, 3f }
    };

    public static MusicLineData GenerateRandomLine()
    {
        MusicLineData line = new MusicLineData();

        for (int measure = 0; measure < line.measureCount; measure++)
        {
            float measureStartBeat = measure * line.beatsPerMeasure;

            float[] chosenPattern = measurePatterns[Random.Range(0, measurePatterns.Count)];

            foreach (float beatInMeasure in chosenPattern)
            {
                NoteEvent note = new NoteEvent();
                note.beatPosition = measureStartBeat + beatInMeasure;
                line.notes.Add(note);
            }
        }

        line.notes.Sort((a, b) => a.beatPosition.CompareTo(b.beatPosition));

        return line;
    }
}
