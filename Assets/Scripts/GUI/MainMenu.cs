using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject mainPausePanel;
    public bool isSettingsOpen = false;
    public string sceneName = "SampleScene";


    private void Start()
    {
        PlayMainMenuMusic();
    }

    private void PlayMainMenuMusic()
    {
        MusicManager.Instance.PlayMainMenu();
    }

    public void PlayGame()
    {
        LoadGame();
        mainPausePanel.SetActive(false);
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

    private void LoadGame()
    {
        LoadingSystem.Instance.LoadLevel(sceneName);
    }

}
