using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private bool isPaused = false;

    [SerializeField] private Settings settings;
    public bool isSettingsOpen = false;

    private void Start()
    {
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        settings.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isSettingsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
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

    void PauseGame()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);
        isPaused = true;
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
            isPaused = false;
        }
    }

    public void Settings()
    {
        if (!isSettingsOpen)
        {
            settings.gameObject.SetActive(true);
            isSettingsOpen = true;
            settings.escClose.SetActive(true);
        }
    }

    public void ExitGame()
    {
        if (!isSettingsOpen)
        {
            Debug.Log("Wracasz do main Menu");
        }
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

