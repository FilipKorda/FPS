using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Tabs")]
    [Space(5)]
    [SerializeField] private SettingsTabHighlight soundTab;
    [SerializeField] private SettingsTabHighlight controlTab;
    [SerializeField] private SettingsTabHighlight gameplayTab;
    [SerializeField] private SettingsTabHighlight graphicsTab;
    [Header("Panels")]
    [Space(5)]
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject graphicsPanel;
    [Header("Hints and MainPanel")]
    [Space(5)]
    [SerializeField] private GameObject settings_Up_Panel;
    [SerializeField] private PauseMenu pauseMenu;
    public GameObject escClose;

    private GameObject[] panels;
    public int currentPanelIndex = 0;

    void Start()
    {
        panels = new GameObject[] { soundPanel, controlsPanel, gameplayPanel, graphicsPanel };
        ShowCurrentPanel();
        UpdateTabHighlight();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escClose.SetActive(false);
            settings_Up_Panel.SetActive(false);
            pauseMenu.isSettingsOpen = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchPanel(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchPanel(1);
        }
    }

    private void SwitchPanel(int direction)
    {
        currentPanelIndex = (currentPanelIndex + direction + panels.Length) % panels.Length;
        ShowCurrentPanel();
        UpdateTabHighlight();
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentPanelIndex);
        }
    }

    public void SetActivePanel(GameObject activePanel)
    {
        soundPanel.SetActive(false);
        controlsPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        graphicsPanel.SetActive(false);

        activePanel.SetActive(true);
    }

    public void SetActivePanelIndex(int index)
    {
        currentPanelIndex = index;
    }

    private void UpdateTabHighlight()
    {
        soundTab.isSelected = currentPanelIndex == 0;
        controlTab.isSelected = currentPanelIndex == 1;
        gameplayTab.isSelected = currentPanelIndex == 2;
        graphicsTab.isSelected = currentPanelIndex == 3;

        soundTab.UpdateHighlight();
        controlTab.UpdateHighlight();
        gameplayTab.UpdateHighlight();
        graphicsTab.UpdateHighlight();
    }
}
