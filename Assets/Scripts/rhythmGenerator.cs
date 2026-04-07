using System.Collections.Generic;
using UnityEngine;

public static class RhythmGenerator
{
    private const int NotesPerMeasure = 4;

    public static MusicLineData GenerateRandomLine()
    {
        MusicLineData line = new MusicLineData();

        // Create tightly grouped notes: 4 notes per measure
        for (int measure = 0; measure < line.measureCount; measure++)
        {
            float measureStartBeat = measure * line.beatsPerMeasure;
            
            // Space 4 notes tightly within the measure
            for (int noteInMeasure = 0; noteInMeasure < NotesPerMeasure; noteInMeasure++)
            {
                NoteEvent note = new NoteEvent();
                // Cluster notes within measure: spread from beat 0.4 to 3.6 for tight grouping
                float beatOffset = 0.4f + (noteInMeasure * 0.8f);
                note.beatPosition = measureStartBeat + beatOffset;
                line.notes.Add(note);
            }
        }

        return line;
    }
}
