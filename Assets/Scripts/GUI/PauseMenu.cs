using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public bool isGamePaused = false;

    [SerializeField] private Settings settings;
    public bool isSettingsOpen = false;

    [SerializeField] private Button[] buttons;

    public string sceneName = "MainMenu";
    private bool isLoading = false;

    private void Start()
    {
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        settings.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isLoading)
        {
            if (!isSettingsOpen)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (isGamePaused)
                    {
                        ResumeGame();
                    }
                    else
                    {
                        PauseGame();
                    }
                }
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (!isSettingsOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            panel.SetActive(false);
            isGamePaused = false;
        }
    }

    public void Settings()
    {
        if (!isSettingsOpen)
        {
            foreach (var button in buttons)
            {
                button.interactable = false;
            }

            settings.gameObject.SetActive(true);
            isSettingsOpen = true;
            settings.escClose.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        isLoading = true;
        LoadingSystem.Instance.LoadLevel(sceneName);
    }


    public void QuitGame()
    {
        if (!isSettingsOpen)
        {
            Debug.Log("wychodzisz z gry");
            Application.Quit();
        }
    }
}

