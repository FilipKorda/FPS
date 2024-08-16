using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private TMP_Dropdown qualityLevelDropdown;
    [SerializeField] private int qualityLevel;    
    private int resetQualityLevel = 2;
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
    [SerializeField] private TMP_Dropdown shadowResolutionDropdown;
    private ShadowResolution[] availableShadowResolutions = {
        ShadowResolution.Low,
        ShadowResolution.Medium,
        ShadowResolution.High,
    };
    [Header("Sounds")]
    [Space(5)]
    [SerializeField] private AudioMixer audioMainMixer;
    //Master
    [SerializeField] private Slider masterSlider;
    [SerializeField] private TextMeshProUGUI masterAmountText;
    private float resetMasterVolume = 5f;
    //music
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicAmountText;
    private float resetMusicVolume = 5f;
    //sfx
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxAmountText;
    private float resetSfxVolume = 5f;
    //Mute
    [SerializeField] private Toggle soundToggle;
    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SfxVolume";
    [SerializeField] private Button[] buttons;

    [Header("Main Menu Adjustmetns")]
    public bool settingsInMainMenu = false;
    [SerializeField] private MainMenu mainMenu;

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

        SetSoundState(soundToggle.isOn);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escClose.SetActive(false);
            settings_Up_Panel.SetActive(false);
            if (pauseMenu != null)
                pauseMenu.isSettingsOpen = false;

            if (settingsInMainMenu)
            {
                foreach (var button in buttons)
                {
                    button.interactable = true;
                }

                mainMenu.isSettingsOpen = false;
            }
            else
            {
                foreach (var button in buttons)
                {
                    button.interactable = true;
                }

                pauseMenu.isSettingsOpen = false;
            }
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
        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.ClearOptions();
            foreach (var level in availableAntiAliasingLevels)
            {
                antiAliasingDropdown.options.Add(new TMP_Dropdown.OptionData(level.ToString()));
            }
            antiAliasingDropdown.RefreshShownValue();
        }
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
            shadowResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
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

    public void SetVolume(string parameterName, float volume)
    {
        audioMainMixer.SetFloat(parameterName, Mathf.Log10(volume) * 20);
    }

    public void OnMasterVolumeChanged(float volume)
    {
        SetVolume(MIXER_MASTER, volume);
        UpdateMasterAmountText(volume);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        SetVolume(MIXER_MUSIC, volume);
        UpdateMusicAmountText(volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        SetVolume(MIXER_SFX, volume);
        UpdateSfxAmountText(volume);
    }

    public void OnSoundToggleChanged(bool isSoundOn)
    {
        SetSoundState(isSoundOn);
    }

    public void SetSoundState(bool isSoundOn)
    {
        if (!isSoundOn)
        {
            SetVolume(MIXER_MASTER, masterSlider.value);
            SetVolume(MIXER_MUSIC, musicSlider.value);
            SetVolume(MIXER_SFX, sfxSlider.value);
        }
        else
        {
            SetVolume(MIXER_MASTER, 0.0001f);
            SetVolume(MIXER_MUSIC, 0.0001f);
            SetVolume(MIXER_SFX, 0.0001f);
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

    public void ResetMaster()
    {
        masterSlider.value = resetMasterVolume;
    }

    public void ResetMusic()
    {
        musicSlider.value = resetMusicVolume;
    }

    public void ResetSfx()
    {
        sfxSlider.value = resetSfxVolume;
    }

    public void ResetMute()
    {
        if (soundToggle != null)
        {
            soundToggle.isOn = false;
        }
        else
        {
            Debug.LogError("Toggle nie jest przypisany. Przypisz Toggle w inspektorze.");
        }
    }

    void UpdateMasterAmountText(float volume)
    {
        if (masterAmountText != null)
        {
            masterAmountText.text = volume.ToString("F0");
        }
    }

    void UpdateMusicAmountText(float volume)
    {
        if (musicAmountText != null)
        {
            musicAmountText.text = volume.ToString("F0");
        }
    }

    void UpdateSfxAmountText(float volume)
    {
        if (sfxAmountText != null)
        {
            sfxAmountText.text = volume.ToString("F0");
        }
    }


    public void ResetQuality()
    {
        qualityLevel = resetQualityLevel; 
        ApplySettings();

        if (qualityLevelDropdown != null)
        {
            qualityLevelDropdown.value = 2;
            qualityLevelDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }



    public void ResetResolutionDropdown()
    {
        if (resolutionDropdown != null)
        {
            resolutionDropdown.value = 0; 
            resolutionDropdown.RefreshShownValue(); 
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }

    public void ResetFullscreenToggle()
    {
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = false;
        }
        else
        {
            Debug.LogError("Toggle nie jest przypisany. Przypisz Toggle w inspektorze.");
        }
    }

    public void ResetAntiAliasingDropdown()
    {
        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.value = 0;
            antiAliasingDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }

    public void ResetShadowResolutionDropdown()
    {
        if (shadowResolutionDropdown != null)
        {
            shadowResolutionDropdown.value = 0;
            shadowResolutionDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }

}
