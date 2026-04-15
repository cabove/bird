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

    private HashSet<string> usedHits = new HashSet<string>();
    private LineSegment lastLine;

    private class HitTarget
    {
        public int noteIndex;
        public float beat;
        public string hitId;
    }

    void Update()
    {
        if (Input.GetKeyDown(hitKey))
        {
            JudgeHit();
        }

        CheckForLineChange();
    }

    void CheckForLineChange()
    {
        if (lineManager == null) return;

        LineSegment currentLine = lineManager.GetCurrentLine();

        if (currentLine != lastLine)
        {
            usedHits.Clear();
            lastLine = currentLine;
        }
    }

    void JudgeHit()
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

        LineSegment currentLine = lineManager.GetCurrentLine();
        if (currentLine == null)
        {
            Debug.Log("No current line.");
            return;
        }

        MusicLineData lineData = currentLine.GetLineData();
        if (lineData == null || lineData.notes == null || lineData.notes.Count == 0)
        {
            Debug.Log("No notes in current line.");
            return;
        }

        float currentBeat = RhythmManager.Instance.GetCurrentBeat();

        List<HitTarget> targets = new List<HitTarget>();

        for (int i = 0; i < lineData.notes.Count; i++)
        {
            NoteEvent note = lineData.notes[i];

            if (note.noteType == NoteType.Quarter)
            {
                string hitId = i + "_0";

                if (!usedHits.Contains(hitId))
                {
                    targets.Add(new HitTarget
                    {
                        noteIndex = i,
                        beat = lineData.startBeat + note.beatPosition,
                        hitId = hitId
                    });
                }
            }
            else if (note.noteType == NoteType.EighthPair)
            {
                string firstHitId = i + "_0";
                string secondHitId = i + "_1";

                if (!usedHits.Contains(firstHitId))
                {
                    targets.Add(new HitTarget
                    {
                        noteIndex = i,
                        beat = lineData.startBeat + note.beatPosition,
                        hitId = firstHitId
                    });
                }

                if (!usedHits.Contains(secondHitId))
                {
                    targets.Add(new HitTarget
                    {
                        noteIndex = i,
                        beat = lineData.startBeat + note.beatPosition + 0.5f,
                        hitId = secondHitId
                    });
                }
            }
        }

        if (targets.Count == 0)
        {
            Debug.Log("No available note found.");
            return;
        }

        HitTarget closestTarget = null;
        float closestAbsDelta = Mathf.Infinity;
        float closestDelta = 0f;

        for (int i = 0; i < targets.Count; i++)
        {
            float delta = currentBeat - targets[i].beat;
            float absDelta = Mathf.Abs(delta);

            if (absDelta < closestAbsDelta)
            {
                closestAbsDelta = absDelta;
                closestDelta = delta;
                closestTarget = targets[i];
            }
        }

        if (closestTarget == null)
        {
            Debug.Log("No available note found.");
            return;
        }

        if (closestAbsDelta > missWindow)
        {
            PlayMissSound();
            Debug.Log("MISS - too far from note");
            return;
        }

        usedHits.Add(closestTarget.hitId);

        string timingSide;
        if (closestDelta < -0.01f)
            timingSide = "EARLY";
        else if (closestDelta > 0.01f)
            timingSide = "LATE";
        else
            timingSide = "ON TIME";

        if (closestAbsDelta <= perfectWindow)
        {
            Debug.Log("PERFECT - " + timingSide + " - delta: " + closestDelta.ToString("F3"));
        }
        else if (closestAbsDelta <= goodWindow)
        {
            Debug.Log("GOOD - " + timingSide + " - delta: " + closestDelta.ToString("F3"));
        }
        else
        {
            Debug.Log("OK - " + timingSide + " - delta: " + closestDelta.ToString("F3"));
        }
    }

    void PlayMissSound()
    {
        if (audioSource != null && missClip != null)
        {
            audioSource.PlayOneShot(missClip);
        }
    }
}