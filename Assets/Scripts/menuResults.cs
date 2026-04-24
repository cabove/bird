using UnityEngine;

public class MenuResults : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject levelSelect;

    public void GoToLevelSelect()
    {
        titleScreen.SetActive(false);
        levelSelect.SetActive(true);
    }
}