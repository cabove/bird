using UnityEngine;

public class LevelSelectMarker : MonoBehaviour
{
    public RectTransform playerMarker;

    public RectTransform tutorialPoint;
    public RectTransform chapter1Point;
    public RectTransform chapter2Point;
    public RectTransform chapter3Point;

    public void MoveToTutorial()
    {
        MoveMarker(tutorialPoint);
    }

    public void MoveToChapter1()
    {
        MoveMarker(chapter1Point);
    }

    public void MoveToChapter2()
    {
        MoveMarker(chapter2Point);
    }

    public void MoveToChapter3()
    {
        MoveMarker(chapter3Point);
    }

    void MoveMarker(RectTransform target)
    {
        if (playerMarker == null || target == null)
            return;

        playerMarker.anchoredPosition = target.anchoredPosition;
    }
}
