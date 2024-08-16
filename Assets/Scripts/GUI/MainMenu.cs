using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private Settings settings;
    public bool isSettingsOpen = false;

    private void Start()
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        settings.gameObject.SetActive(false);
    }


    public void PlayGame()
    {
        panel.SetActive(false);
        Debug.Log("Play Game");
    }

    public void Settings()
    {
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


}
