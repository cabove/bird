using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject levelSelectScreen;
    public GameObject gameplayRoot;

    public GameplayStarter gameplayStarter;

    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
        gameplayRoot.SetActive(false);
    }

    public void ShowLevelSelectScreen()
    {
        titleScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
        gameplayRoot.SetActive(false);
    }

    public void StartLevel1()
    {
        Time.timeScale = 1f;

        titleScreen.SetActive(false);
        levelSelectScreen.SetActive(false);
        gameplayRoot.SetActive(true);

        if (gameplayStarter != null)
        {
            gameplayStarter.StartLevel();
        }
    }
}
