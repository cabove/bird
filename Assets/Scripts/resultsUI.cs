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

        int early = 0;
        int onTime = 0;
        int late = 0;
        int miss = 0;

        foreach (var h in hits)
        {
            if (h.judgment == HitJudge.Judgment.Early) early++;
            else if (h.judgment == HitJudge.Judgment.OnTime) onTime++;
            else if (h.judgment == HitJudge.Judgment.Late) late++;
            else if (h.judgment == HitJudge.Judgment.Miss) miss++;
        }

        // You can keep using the same text boxes in the Inspector.
        perfectText.text = "Early: " + early;
        goodText.text = "On Time: " + onTime;
        okText.text = "Late: " + late;
        missText.text = "Miss: " + miss;

        var measures = hitJudge.GetMeasuresWithMistakes();

        if (measures.Count == 0)
        {
            badMeasuresText.text = "Measures with mistakes: None";
        }
        else
        {
            for (int i = 0; i < measures.Count; i++)
                measures[i] += 1;

            badMeasuresText.text = "Measures with mistakes: " + string.Join(", ", measures);
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