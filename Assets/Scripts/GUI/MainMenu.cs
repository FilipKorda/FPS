using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private Button[] buttons;
    public bool isSettingsOpen = false;
    public string sceneName = "SampleScene";

    public void PlayGame()
    {
        Debug.Log("Play Game");
    }

    public void Settings()
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
        settings.gameObject.SetActive(true);
        isSettingsOpen = true;
    }

    public void QuitGame()
    {
        if (!isSettingsOpen)
        {
            Debug.Log("wychodzisz z gry");
            Application.Quit();
        }
    }

    public void LoadGame()
    {
        LoadingSystem.Instance.LoadLevel(sceneName);
    }

}
