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

    private HashSet<int> usedNotes = new HashSet<int>();
    private LineSegment lastLine;

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
            usedNotes.Clear();
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

        int closestIndex = -1;
        float closestAbsDelta = Mathf.Infinity;
        float closestDelta = 0f;

        for (int i = 0; i < lineData.notes.Count; i++)
        {
            if (usedNotes.Contains(i))
                continue;

            float noteBeat = lineData.startBeat + lineData.notes[i].beatPosition;
            float delta = currentBeat - noteBeat;
            float absDelta = Mathf.Abs(delta);

            if (absDelta < closestAbsDelta)
            {
                closestAbsDelta = absDelta;
                closestDelta = delta;
                closestIndex = i;
            }
        }

        if (closestIndex == -1)
        {
            Debug.Log("No available note found.");
            return;
        }

        if (closestAbsDelta > missWindow)
        {
            Debug.Log("MISS - too far from note");
            return;
        }

        usedNotes.Add(closestIndex);

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
}
