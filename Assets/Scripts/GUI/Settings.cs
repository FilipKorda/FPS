using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
    private GameObject[] panels;
    [Header("Hints and MainPanel")]
    [Space(5)]
    [SerializeField] private GameObject settings_Up_Panel;
    [SerializeField] private PauseMenu pauseMenu;
    public GameObject escClose;
    public int currentPanelIndex = 0;
    [Header("Gameplay")]
    [Space(5)]
    [SerializeField] private FPSDisplay fPSDisplay;
    [Header("Graphic")]
    [Space(5)]
    [SerializeField] private int qualityLevel;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    private Resolution[] customResolutions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720, refreshRate = 60 }, 
        new Resolution { width = 1920, height = 1080, refreshRate = 60 }, 
        new Resolution { width = 2560, height = 1440, refreshRate = 60 }   
    };
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private int[] availableAntiAliasingLevels = { 0, 2, 4, 8 };
    [SerializeField] private Dropdown shadowResolutionDropdown;
    private ShadowResolution[] availableShadowResolutions = {
        ShadowResolution.Low,
        ShadowResolution.Medium,
        ShadowResolution.High,
    };


    void Start()
    {
        panels = new GameObject[] { soundPanel, controlsPanel, gameplayPanel, graphicsPanel };

        qualityLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        ApplySettings();

        ShowCurrentPanel();
        UpdateTabHighlight();

        InitializeResolutionOptions();

        InitializeAntiAliasing();
        SetAntiAliasingLevel(availableAntiAliasingLevels[0]);

        InitializeShadow();
        SetShadowResolution(availableShadowResolutions[0]);
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

    public void SetQualityLevel(int level)
    {
        qualityLevel = level;
        ApplySettings();
    }

    void ApplySettings()
    {
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
    }

    void InitializeResolutionOptions()
    {
        resolutionDropdown.ClearOptions();

        foreach (Resolution resolution in customResolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + "x" + resolution.height));
        }

        resolutionDropdown.value = 0;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution()
    {
        int resolutionIndex = resolutionDropdown.value;

        if (resolutionIndex >= 0 && resolutionIndex < customResolutions.Length)
        {
            Resolution selectedResolution = customResolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        }
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    void InitializeAntiAliasing()
    {
        antiAliasingDropdown.ClearOptions();
        foreach (var level in availableAntiAliasingLevels)
        {
            antiAliasingDropdown.options.Add(new TMP_Dropdown.OptionData(level.ToString()));
        }
        antiAliasingDropdown.RefreshShownValue();
    }

    void SetAntiAliasingLevel(int level)
    {
        QualitySettings.antiAliasing = level;
    }

    public void OnAntiAliasingDropdownValueChanged(int index)
    {
        int selectedLevel = availableAntiAliasingLevels[index];
        SetAntiAliasingLevel(selectedLevel);
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentPanelIndex);
        }
    }


    void InitializeShadow()
    {
        shadowResolutionDropdown.ClearOptions();
        foreach (var resolution in availableShadowResolutions)
        {
            shadowResolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        shadowResolutionDropdown.RefreshShownValue();
    }

    void SetShadowResolution(ShadowResolution resolution)
    {
        QualitySettings.shadowResolution = resolution;
    }

    public void OnShadowResolutionDropdownValueChanged(int index)
    {
        ShadowResolution selectedResolution = availableShadowResolutions[index];
        SetShadowResolution(selectedResolution);
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
