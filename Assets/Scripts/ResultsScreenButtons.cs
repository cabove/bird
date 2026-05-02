using UnityEngine;

public class ResultsScreenButtons : MonoBehaviour
{
    [Header("Panels")]
    public GameObject resultsPanel;
    public GameObject resultsDetail;
    public GameObject mainMenuPanel;

    public void HideResultsDetail()
    {
        if (resultsDetail != null)
        {
            resultsDetail.SetActive(false);
        }
    }

    public void GoToMainMenu()
    {
        // Hide results detail first, in case it is open
        if (resultsDetail != null)
        {
            resultsDetail.SetActive(false);
        }

        // Hide the full results screen
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(false);
        }

        // Show the main menu screen
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }
}
