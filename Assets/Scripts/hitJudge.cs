using System.Collections.Generic;
using UnityEngine;

public class HitJudge : MonoBehaviour
{
    public LineManager lineManager;

    [Header("Input")]
    public KeyCode hitKey = KeyCode.Space;

    [Header("Timing Windows - in beats")]
    public float onTimeWindow = 0.10f;
    public float hitWindow = 0.35f;

    [Header("Colors")]
    public Color earlyColor = Color.red;
    public Color onTimeColor = Color.green;
    public Color lateColor = Color.blue;
    public Color missColor = Color.gray;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip missClip;

    private Queue<HitTarget> upcomingHits = new Queue<HitTarget>();
    private LineSegment lastLine;

    private List<JudgedHit> judgedHits = new List<JudgedHit>();

    private class HitTarget
    {
        public int noteIndex;
        public float beat;
        public string hitId;
        public int measureIndex;
    }

    public enum Judgment
    {
        Early,
        OnTime,
        Late,
        Miss
    }

    [System.Serializable]
    public class JudgedHit
    {
        public string hitId;
        public int noteIndex;
        public int measureIndex;
        public float beat;
        public float delta;
        public Judgment judgment;
    }

    void Update()
    {
        CheckForLineChange();
        AutoMissExpiredFrontNotes();

        if (Input.GetKeyDown(hitKey))
        {
            JudgeFrontHit();
        }
    }

    void CheckForLineChange()
    {
        if (lineManager == null) return;

        LineSegment currentLine = lineManager.GetCurrentLine();
        if (currentLine == lastLine) return;

        lastLine = currentLine;
        BuildQueueForCurrentLine();
    }

    void BuildQueueForCurrentLine()
    {
        upcomingHits.Clear();

        if (lastLine == null) return;

        MusicLineData lineData = lastLine.GetLineData();
        if (lineData == null || lineData.notes == null) return;

        for (int i = 0; i < lineData.notes.Count; i++)
        {
            NoteEvent note = lineData.notes[i];

            int measureIndex = Mathf.FloorToInt(note.beatPosition / lineData.beatsPerMeasure);

            if (note.noteType == NoteType.Quarter)
            {
                upcomingHits.Enqueue(new HitTarget
                {
                    noteIndex = i,
                    beat = lineData.startBeat + note.beatPosition,
                    hitId = i + "_0",
                    measureIndex = measureIndex
                });
            }
            else if (note.noteType == NoteType.EighthPair)
            {
                upcomingHits.Enqueue(new HitTarget
                {
                    noteIndex = i,
                    beat = lineData.startBeat + note.beatPosition,
                    hitId = i + "_0",
                    measureIndex = measureIndex
                });

                upcomingHits.Enqueue(new HitTarget
                {
                    noteIndex = i,
                    beat = lineData.startBeat + note.beatPosition + 0.5f,
                    hitId = i + "_1",
                    measureIndex = measureIndex
                });
            }
        }
    }

    void AutoMissExpiredFrontNotes()
    {
        if (RhythmManager.Instance == null) return;

        float currentBeat = RhythmManager.Instance.GetCurrentBeat();

        while (upcomingHits.Count > 0)
        {
            HitTarget front = upcomingHits.Peek();

            if (currentBeat > front.beat + hitWindow)
            {
                RegisterJudgment(front, currentBeat - front.beat, Judgment.Miss);
                ColorNote(front, Judgment.Miss);
                PlayMissSound();
                Debug.Log("MISS - no input - measure " + (front.measureIndex + 1));
                upcomingHits.Dequeue();
            }
            else
            {
                break;
            }
        }
    }

    void JudgeFrontHit()
    {
        if (lineManager == null)
        {
            Debug.LogWarning("HitJudge: LineManager is not assigned.");
            return;
        }

        if (RhythmManager.Instance == null)
        {
            Debug.LogWarning("HitJudge: RhythmManager.Instance is missing.");
            return;
        }

        if (upcomingHits.Count == 0)
        {
            Debug.Log("No available note found.");
            return;
        }

        float currentBeat = RhythmManager.Instance.GetCurrentBeat();
        HitTarget front = upcomingHits.Peek();

        float delta = currentBeat - front.beat;
        float absDelta = Mathf.Abs(delta);

        if (absDelta > hitWindow)
        {
            PlayMissSound();
            Debug.Log("MISS - too far from next note");
            return;
        }

        Judgment result;

        if (absDelta <= onTimeWindow)
        {
            result = Judgment.OnTime;
            Debug.Log("ON TIME - delta: " + delta.ToString("F3"));
        }
        else if (delta < 0f)
        {
            result = Judgment.Early;
            Debug.Log("EARLY - delta: " + delta.ToString("F3"));
        }
        else
        {
            result = Judgment.Late;
            Debug.Log("LATE - delta: " + delta.ToString("F3"));
        }

        RegisterJudgment(front, delta, result);
        ColorNote(front, result);
        upcomingHits.Dequeue();
    }

    void RegisterJudgment(HitTarget target, float delta, Judgment judgment)
    {
        judgedHits.Add(new JudgedHit
        {
            hitId = target.hitId,
            noteIndex = target.noteIndex,
            measureIndex = target.measureIndex,
            beat = target.beat,
            delta = delta,
            judgment = judgment
        });
    }

    void ColorNote(HitTarget target, Judgment judgment)
    {
        if (lastLine == null) return;

        if (judgment == Judgment.Early)
        {
            lastLine.ColorHit(target.hitId, earlyColor);
        }
        else if (judgment == Judgment.OnTime)
        {
            lastLine.ColorHit(target.hitId, onTimeColor);
        }
        else if (judgment == Judgment.Late)
        {
            lastLine.ColorHit(target.hitId, lateColor);
        }
        else if (judgment == Judgment.Miss)
        {
            lastLine.ColorHit(target.hitId, missColor);
        }
    }

    void PlayMissSound()
    {
        if (audioSource != null && missClip != null)
        {
            audioSource.PlayOneShot(missClip);
        }
    }

    public List<JudgedHit> GetJudgedHits()
    {
        return judgedHits;
    }

    public List<int> GetMeasuresWithMistakes()
    {
        HashSet<int> badMeasures = new HashSet<int>();

        foreach (var h in judgedHits)
        {
            if (h.judgment == Judgment.Early || h.judgment == Judgment.Late || h.judgment == Judgment.Miss)
            {
                badMeasures.Add(h.measureIndex);
            }
        }

        return new List<int>(badMeasures);
    }
}