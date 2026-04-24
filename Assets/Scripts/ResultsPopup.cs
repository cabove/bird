using UnityEngine;

public class ResultsPopup : MonoBehaviour
{
    public GameObject fullReportPopup;

    public void ShowPopup()
    {
        fullReportPopup.SetActive(true);
    }

    public void HidePopup()
    {
        fullReportPopup.SetActive(false);
    }
}