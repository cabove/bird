using UnityEngine;

public class LevelSelectMarker : MonoBehaviour
{
    public RectTransform playerMarker;

    public RectTransform tutorialPoint;
    public RectTransform chapter1Point;
    public RectTransform chapter2Point;
    public RectTransform chapter3Point;

    public ScreenManager screenManager;

    private int selectedChapter = -1;

    public void OnTutorialClicked()
    {
        Debug.Log("Tutorial clicked. Selected = " + selectedChapter);

        if (selectedChapter == 0)
        {
            Debug.Log("Tutorial clicked again");
            // add tutorial start later if you want
        }
        else
        {
            selectedChapter = 0;
            MoveMarker(tutorialPoint);
            Debug.Log("Moved to Tutorial");
        }
    }

    public void OnChapter1Clicked()
    {
        Debug.Log("Chapter 1 clicked. Selected = " + selectedChapter);

        if (selectedChapter == 1)
        {
            Debug.Log("Second click on Chapter 1 -> Start Level 1");
            screenManager.StartLevel1();
        }
        else
        {
            selectedChapter = 1;
            MoveMarker(chapter1Point);
            Debug.Log("Moved to Chapter 1");
        }
    }

    public void OnChapter2Clicked()
    {
        Debug.Log("Chapter 2 clicked. Selected = " + selectedChapter);

        if (selectedChapter == 2)
        {
            Debug.Log("Chapter 2 clicked again");
        }
        else
        {
            selectedChapter = 2;
            MoveMarker(chapter2Point);
            Debug.Log("Moved to Chapter 2");
        }
    }

    public void OnChapter3Clicked()
    {
        Debug.Log("Chapter 3 clicked. Selected = " + selectedChapter);

        if (selectedChapter == 3)
        {
            Debug.Log("Chapter 3 clicked again");
        }
        else
        {
            selectedChapter = 3;
            MoveMarker(chapter3Point);
            Debug.Log("Moved to Chapter 3");
        }
    }

    void MoveMarker(RectTransform target)
    {
        if (playerMarker == null || target == null)
        {
            Debug.Log("Marker or target missing");
            return;
        }

        playerMarker.anchoredPosition = target.anchoredPosition;
    }
}