using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsUI : MonoBehaviour
{
    public HitJudge hitJudge;

    public TMP_Text perfectText;
    public TMP_Text goodText;
    public TMP_Text okText;
    public TMP_Text missText;
    public TMP_Text badMeasuresText;

    void OnEnable()
    {
        ShowResults();
    }

    void ShowResults()
    {
        var hits = hitJudge.GetJudgedHits();

        int perfect = 0;
        int good = 0;
        int ok = 0;
        int miss = 0;

        foreach (var h in hits)
        {
            if (h.judgment == HitJudge.Judgment.Perfect) perfect++;
            else if (h.judgment == HitJudge.Judgment.Good) good++;
            else if (h.judgment == HitJudge.Judgment.Ok) ok++;
            else if (h.judgment == HitJudge.Judgment.Miss) miss++;
        }

        perfectText.text = "Perfect: " + perfect;
        goodText.text = "Good: " + good;
        okText.text = "OK: " + ok;
        missText.text = "Miss: " + miss;

        var measures = hitJudge.GetMeasuresWithMistakes();

        if (measures.Count == 0)
        {
            badMeasuresText.text = "Measures: None";
        }
        else
        {
            for (int i = 0; i < measures.Count; i++)
                measures[i] += 1;

            badMeasuresText.text = "Measures: " + string.Join(", ", measures);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
