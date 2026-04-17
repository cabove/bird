using System.Collections.Generic;
using UnityEngine;

public class HitJudge : MonoBehaviour
{
    public LineManager lineManager;

    [Header("Input")]
    public KeyCode hitKey = KeyCode.Space;

    [Header("Timing Windows (in beats)")]
    public float perfectWindow = 0.10f;
    public float goodWindow = 0.20f;
    public float missWindow = 0.35f;

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
        Perfect,
        Good,
        Ok,
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

                // second eighth still belongs to same measure in your current design
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

            if (currentBeat > front.beat + missWindow)
            {
                RegisterJudgment(front, currentBeat - front.beat, Judgment.Miss);
                PlayMissSound();
                Debug.Log($"MISS - no input - measure {front.measureIndex + 1}");
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

        if (absDelta > missWindow)
        {
            PlayMissSound();
            Debug.Log("MISS - too far from next note");
            return;
        }

        string timingSide;
        if (delta < -0.01f)
            timingSide = "EARLY";
        else if (delta > 0.01f)
            timingSide = "LATE";
        else
            timingSide = "ON TIME";

        Judgment result;

        if (absDelta <= perfectWindow)
        {
            result = Judgment.Perfect;
            Debug.Log("PERFECT - " + timingSide + " - delta: " + delta.ToString("F3"));
        }
        else if (absDelta <= goodWindow)
        {
            result = Judgment.Good;
            Debug.Log("GOOD - " + timingSide + " - delta: " + delta.ToString("F3"));
        }
        else
        {
            result = Judgment.Ok;
            Debug.Log("OK - " + timingSide + " - delta: " + delta.ToString("F3"));
        }

        RegisterJudgment(front, delta, result);
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

        for (int i = 0; i < judgedHits.Count; i++)
        {
            if (judgedHits[i].judgment == Judgment.Ok || judgedHits[i].judgment == Judgment.Miss)
            {
                badMeasures.Add(judgedHits[i].measureIndex);
            }
        }

        return new List<int>(badMeasures);
    }
}