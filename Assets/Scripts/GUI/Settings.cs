using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Game.Audio;

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
    [Header("Controls")]
    [Space(5)]
    [SerializeField] private Toggle invertMouseToggle;
    [SerializeField] private MouseLook mouseLook;
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
    private float resetMasterVolume = 1f;
    //music
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicAmountText;
    private float resetMusicVolume = 1f;
    //sfx
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxAmountText;
    private float resetSfxVolume = 1f;
    //Mute
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Button[] buttons;

    [Header("Main Menu Adjustmetns")]
    public bool settingsInMainMenu = false;
    [SerializeField] private MainMenu mainMenu;

    [Header("Localization")]
    [SerializeField] private string uiStringTable = "UI";
    [SerializeField] private TMP_Dropdown languageDropdown;

    private readonly List<Locale> languageOptions = new List<Locale>();
    private const string LocalePrefKey = "SelectedLocaleCode";

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        ReadingSoundsSaveValues();
        ReadingToggleMuteSaveValue();
        SetSoundState(soundToggle != null ? soundToggle.isOn : false); 
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    void Start()
    {
        panels = new GameObject[] { soundPanel, controlsPanel, gameplayPanel, graphicsPanel };

        qualityLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());

        ApplySettings();

        ShowCurrentPanel();
        UpdateTabHighlight();

        RefreshQualityDropdownLocalization();

        InitializeResolutionOptions();

        InitializeAntiAliasing();
        SetAntiAliasingLevel(availableAntiAliasingLevels[0]);

        InitializeShadow();
        SetShadowResolution(availableShadowResolutions[0]);

        ReadingSoundsSaveValues();
        ReadingToggleMuteSaveValue(); 
        SetSoundState(soundToggle != null ? soundToggle.isOn : false); 

        ReadingQualitySaveValues();
        ReadingResolutionSaveValues();
        ReadingAntiAliasingSaveValues();
        ReadingShadowSaveValues();
        ReadingToggleFullscreenSaveValue();

        ReadingInvertMouseSaveValue();
        if (invertMouseToggle != null)
            invertMouseToggle.onValueChanged.AddListener(OnInvertMouseToggleChanged);

        StartCoroutine(InitializeLanguageDropdown());
    }

    private static string GetLanguageKey(string code)
    {
        if (string.IsNullOrEmpty(code)) return string.Empty;
        int dash = code.IndexOf('-');
        return (dash >= 0 ? code.Substring(0, dash) : code).ToLowerInvariant();
    }

    private int FindLanguageIndexByCode(string code)
    {
        if (string.IsNullOrEmpty(code) || languageOptions.Count == 0) return -1;

        for (int i = 0; i < languageOptions.Count; i++)
        {
            if (string.Equals(languageOptions[i].Identifier.Code, code))
                return i;
        }

        string lang = GetLanguageKey(code);
        for (int i = 0; i < languageOptions.Count; i++)
        {
            if (GetLanguageKey(languageOptions[i].Identifier.Code) == lang)
                return i;
        }

        return -1;
    }

    private IEnumerator InitializeLanguageDropdown()
    {
        if (languageDropdown == null)
            yield break;

        var init = LocalizationSettings.InitializationOperation;
        if (!init.IsDone)
            yield return init;

        languageDropdown.ClearOptions();
        languageOptions.Clear();

        var allLocales = LocalizationSettings.AvailableLocales != null
            ? LocalizationSettings.AvailableLocales.Locales
            : null;

        if (allLocales == null || allLocales.Count == 0)
            yield break;

        Locale pl = null, en = null;
        foreach (var locale in allLocales)
        {
            var lang = GetLanguageKey(locale.Identifier.Code);
            if (lang == "pl") pl = pl ?? locale;
            else if (lang == "en") en = en ?? locale;
        }
        if (pl != null) languageOptions.Add(pl);
        if (en != null) languageOptions.Add(en);

        var options = new List<TMP_Dropdown.OptionData>(languageOptions.Count);
        foreach (var locale in languageOptions)
        {
            var ci = locale.Identifier.CultureInfo;
            string label = locale.Identifier.Code;

            if (ci != null)
            {
                var name = ci.EnglishName;
                int paren = name.IndexOf(" (", StringComparison.Ordinal);
                label = paren > 0 ? name.Substring(0, paren) : name;
            }

            if (string.IsNullOrWhiteSpace(label))
                label = GetLanguageKey(locale.Identifier.Code);

            options.Add(new TMP_Dropdown.OptionData(label));
        }
        languageDropdown.AddOptions(options);

        string savedCode = null;
        var selectors = LocalizationSettings.StartupLocaleSelectors;
        foreach (var s in selectors)
        {
            if (s is PlayerPrefLocaleSelector pp && !string.IsNullOrEmpty(pp.PlayerPreferenceKey) && PlayerPrefs.HasKey(pp.PlayerPreferenceKey))
            {
                savedCode = PlayerPrefs.GetString(pp.PlayerPreferenceKey);
                break;
            }
        }

        if (string.IsNullOrEmpty(savedCode) && PlayerPrefs.HasKey(LocalePrefKey))
            savedCode = PlayerPrefs.GetString(LocalePrefKey);

        if (string.IsNullOrEmpty(savedCode) && LocalizationSettings.SelectedLocale != null)
            savedCode = LocalizationSettings.SelectedLocale.Identifier.Code;

        Locale targetLocale = null;
        int dropdownIndex = 0;

        if (!string.IsNullOrEmpty(savedCode))
        {
            int idx = FindLanguageIndexByCode(savedCode);
            if (idx >= 0)
            {
                targetLocale = languageOptions[idx];
                dropdownIndex = idx;
            }
        }

        if (targetLocale == null)
        {
            int plIdx = FindLanguageIndexByCode("pl");
            if (plIdx >= 0)
            {
                targetLocale = languageOptions[plIdx];
                dropdownIndex = plIdx;
            }
            else if (languageOptions.Count > 0)
            {
                targetLocale = languageOptions[0];
                dropdownIndex = 0;
            }
        }

        if (targetLocale != null)
            PersistSelectedLocale(targetLocale);

        languageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownValueChanged);
        languageDropdown.value = dropdownIndex;
        languageDropdown.RefreshShownValue();
        languageDropdown.onValueChanged.AddListener(OnLanguageDropdownValueChanged);

        SyncLanguageDropdownWithSelectedLocale();
    }

    public void OnLanguageDropdownValueChanged(int index)
    {
        if (index < 0 || index >= languageOptions.Count) return;

        var selected = languageOptions[index];
        if (selected != null && LocalizationSettings.SelectedLocale != selected)
        {
            PersistSelectedLocale(selected);
        }
    }

    private void SyncLanguageDropdownWithSelectedLocale()
    {
        if (languageDropdown == null || languageOptions.Count == 0) return;
        var current = LocalizationSettings.SelectedLocale;
        if (current == null) return;

        int idx = FindLanguageIndexByCode(current.Identifier.Code);
        if (idx >= 0 && languageDropdown.value != idx)
        {
            languageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownValueChanged);
            languageDropdown.value = idx;
            languageDropdown.RefreshShownValue();
            languageDropdown.onValueChanged.AddListener(OnLanguageDropdownValueChanged);
        }
    }

    public void ResetLanguageDropdown()
    {
        if (languageDropdown == null)
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
            return;
        }

        PlayerPrefs.DeleteKey(LocalePrefKey);

        Locale target = null;
        int idx = FindLanguageIndexByCode("pl");
        if (idx >= 0) target = languageOptions[idx];
        else if (languageOptions.Count > 0) { target = languageOptions[0]; idx = 0; }

        if (target != null)
        {
            PersistSelectedLocale(target);

            languageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownValueChanged);
            languageDropdown.value = Mathf.Max(0, idx);
            languageDropdown.RefreshShownValue();
            languageDropdown.onValueChanged.AddListener(OnLanguageDropdownValueChanged);
        }
    }

    public void ResetInvertMouse()
    {
        bool defaultInvert = false;
        PlayerPrefs.DeleteKey("InvertMouse"); 
        PlayerPrefs.SetInt("InvertMouse", defaultInvert ? 1 : 0); 

        if (invertMouseToggle != null)
            invertMouseToggle.isOn = defaultInvert;

        if (mouseLook != null)
            mouseLook.SetInvert(defaultInvert);
    }

    public void ResetMouseInvertAndLanguageSettings()
    {
        ResetLanguageDropdown();
        ResetInvertMouse();
    }

    void ReadingSoundsSaveValues()
    {
        float savedMaster = PlayerPrefs.GetFloat(AudioKeys.PlayerPrefMasterVolume, resetMasterVolume);
        float savedMusic = PlayerPrefs.GetFloat(AudioKeys.PlayerPrefMusicVolume, resetMusicVolume);
        float savedSfx = PlayerPrefs.GetFloat(AudioKeys.PlayerPrefSfxVolume, resetSfxVolume);

        if (masterSlider != null)
            masterSlider.value = (masterSlider.maxValue > 1f) ? Mathf.Clamp01(savedMaster) * masterSlider.maxValue : Mathf.Clamp01(savedMaster);

        if (musicSlider != null)
            musicSlider.value = (musicSlider.maxValue > 1f) ? Mathf.Clamp01(savedMusic) * musicSlider.maxValue : Mathf.Clamp01(savedMusic);

        if (sfxSlider != null)
            sfxSlider.value = (sfxSlider.maxValue > 1f) ? Mathf.Clamp01(savedSfx) * sfxSlider.maxValue : Mathf.Clamp01(savedSfx);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
          ResetMouseInvertAndLanguageSettings();
        }

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

    public void SetQualityLevel(int level)
    {
        qualityLevel = level;
        ApplySettings();

        PlayerPrefs.SetInt("QualityLevelDropdown", level);
    }

    void ApplySettings()
    {
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
    }

    private void ReadingQualitySaveValues()
    {
        if (PlayerPrefs.HasKey("QualityLevelDropdown"))
        {
            int savedIndex = PlayerPrefs.GetInt("QualityLevelDropdown");
            qualityLevelDropdown.value = savedIndex;
        }
        else
        {
            qualityLevelDropdown.value = 2;
        }
        qualityLevelDropdown.RefreshShownValue();
    }

    public void ResetQuality()
    {
        qualityLevel = resetQualityLevel;

        ApplySettings();

        if (qualityLevelDropdown != null)
        {
            PlayerPrefs.DeleteKey("QualityLevelDropdown");
            qualityLevelDropdown.value = 2;
            qualityLevelDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
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
        PlayerPrefs.SetInt("ResolutionDropdown", resolutionIndex);
    }

    private void ReadingResolutionSaveValues()
    {
        if (PlayerPrefs.HasKey("ResolutionDropdown"))
        {
            int savedIndex = PlayerPrefs.GetInt("ResolutionDropdown");
            resolutionDropdown.value = savedIndex;
        }
    }

    public void ResetResolutionDropdown()
    {
        if (resolutionDropdown != null)
        {
            PlayerPrefs.DeleteKey("ResolutionDropdown");
            resolutionDropdown.value = 0;
            resolutionDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }

    public void OnFullscreenToggleValueChanged(bool isOn)
    {
        Screen.fullScreen = isOn;

        PlayerPrefs.SetInt("ToggleFullscreen", isOn ? 1 : 0);
    }

    public void ResetFullscreenToggle()
    {
        if (fullscreenToggle != null)
        {
            Screen.fullScreen = false;
            PlayerPrefs.DeleteKey("ToggleFullscreen");
            fullscreenToggle.isOn = false;
        }
        else
        {
            Debug.LogError("Toggle nie jest przypisany. Przypisz Toggle w inspektorze.");
        }
    }

    private void ReadingToggleFullscreenSaveValue()
    {
        if (PlayerPrefs.HasKey("ToggleFullscreen"))
        {
            int savedState = PlayerPrefs.GetInt("ToggleFullscreen");
            bool isFullscreen = savedState == 1;
            Screen.fullScreen = isFullscreen;

            if (fullscreenToggle != null)
            {
                fullscreenToggle.isOn = isFullscreen;
            }
        }
        else
        {
            Screen.fullScreen = false;
            if (fullscreenToggle != null)
            {
                fullscreenToggle.isOn = false;
            }
        }
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
        PlayerPrefs.SetInt("AntiAliasingDropdown", index);
    }

    private void ReadingAntiAliasingSaveValues()
    {
        if (PlayerPrefs.HasKey("AntiAliasingDropdown"))
        {
            int savedIndex = PlayerPrefs.GetInt("AntiAliasingDropdown");
            antiAliasingDropdown.value = savedIndex;
        }
    }

    public void ResetAntiAliasingDropdown()
    {
        if (antiAliasingDropdown != null)
        {
            PlayerPrefs.DeleteKey("AntiAliasingDropdown");
            antiAliasingDropdown.value = 0;
            antiAliasingDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }

    void InitializeShadow()
    {
        RefreshShadowDropdownLocalization();
    }

    void SetShadowResolution(ShadowResolution resolution)
    {
        QualitySettings.shadowResolution = resolution;
    }

    public void OnShadowResolutionDropdownValueChanged(int index)
    {
        ShadowResolution selectedResolution = availableShadowResolutions[index];
        SetShadowResolution(selectedResolution);
        PlayerPrefs.SetInt("ShadowResolutionDropdown", index);
    }

    private void ReadingShadowSaveValues()
    {
        if (PlayerPrefs.HasKey("ShadowResolutionDropdown"))
        {
            int savedIndex = PlayerPrefs.GetInt("ShadowResolutionDropdown");
            shadowResolutionDropdown.value = savedIndex;
        }
    }

    public void ResetShadowResolutionDropdown()
    {
        if (shadowResolutionDropdown != null)
        {
            PlayerPrefs.DeleteKey("ShadowResolutionDropdown");
            shadowResolutionDropdown.value = 0;
            shadowResolutionDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }

    public void SetVolume(string parameterName, float volume)
    {
        if (audioMainMixer == null) return;

        float linear = (volume > 1f) ? Mathf.Clamp01(volume / 100f) : Mathf.Clamp01(volume);

        if (linear <= 0f)
        {
            audioMainMixer.SetFloat(parameterName, -80f);
        }
        else
        {
            float db = Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
            audioMainMixer.SetFloat(parameterName, db);
        }
    }

    public void OnMasterVolumeChanged(float volume)
    {
        SetVolume(AudioKeys.MixerMasterParam, volume);
        UpdateMasterAmountText(volume);

        float linear = (volume > 1f)
            ? Mathf.Clamp01(volume / (masterSlider != null ? masterSlider.maxValue : 100f))
            : Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(AudioKeys.PlayerPrefMasterVolume, linear);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        SetVolume(AudioKeys.MixerMusicParam, volume);
        UpdateMusicAmountText(volume);

        float linear = (volume > 1f) ? Mathf.Clamp01(volume / (musicSlider != null ? musicSlider.maxValue : 100f)) : Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(AudioKeys.PlayerPrefMusicVolume, linear);

        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(linear);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        SetVolume(AudioKeys.MixerSfxParam, volume);
        UpdateSfxAmountText(volume);

        float linear = (volume > 1f)
            ? Mathf.Clamp01(volume / (sfxSlider != null ? sfxSlider.maxValue : 100f))
            : Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(AudioKeys.PlayerPrefSfxVolume, linear);
    }

    public void OnSoundToggleChanged(bool isMuted)
    {
        PlayerPrefs.SetInt(AudioKeys.PlayerPrefToggleMute, isMuted ? 1 : 0);

        SetSoundState(isMuted);
    }

    private void ReadingToggleMuteSaveValue()
    {
        if (PlayerPrefs.HasKey(AudioKeys.PlayerPrefToggleMute))
        {
            int savedState = PlayerPrefs.GetInt(AudioKeys.PlayerPrefToggleMute);
            if (soundToggle != null)
                soundToggle.isOn = savedState == 1; 
        }
    }

    public void ResetMute()
    {
        if (soundToggle != null)
        {
            PlayerPrefs.DeleteKey(AudioKeys.PlayerPrefToggleMute);
            
            soundToggle.isOn = false;
            SetSoundState(false);
        }
        else
        {
            Debug.LogError("Toggle nie jest przypisany. Przypisz Toggle w inspektorze.");
        }
    }

    public void SetSoundState(bool isMuted)
    {
        if (isMuted)
        {
            SetVolume(AudioKeys.MixerMasterParam, 0f);
            SetVolume(AudioKeys.MixerMusicParam, 0f);
            SetVolume(AudioKeys.MixerSfxParam, 0f);
            if (MusicManager.Instance != null)
                MusicManager.Instance.SetVolume(0f);
        }
        else
        {
            SetVolume(AudioKeys.MixerMasterParam, masterSlider != null ? masterSlider.value : resetMasterVolume);
            SetVolume(AudioKeys.MixerMusicParam, musicSlider != null ? musicSlider.value : resetMusicVolume);
            SetVolume(AudioKeys.MixerSfxParam, sfxSlider != null ? sfxSlider.value : resetSfxVolume);

            float musicLinear = (musicSlider != null)
                ? ((musicSlider.value > 1f) ? Mathf.Clamp01(musicSlider.value / musicSlider.maxValue) : Mathf.Clamp01(musicSlider.value))
                : resetMusicVolume;
            if (MusicManager.Instance != null)
                MusicManager.Instance.SetVolume(musicLinear);
        }
    }

    public void ResetMaster()
    {
        masterSlider.value = resetMasterVolume;
        PlayerPrefs.DeleteKey(AudioKeys.PlayerPrefMasterVolume);
        masterSlider.value = resetMasterVolume;
        SetVolume(AudioKeys.MixerMasterParam, resetMasterVolume);
        UpdateMasterAmountText(resetMasterVolume);
    }

    public void ResetMusic()
    {
        musicSlider.value = resetMusicVolume;
        PlayerPrefs.DeleteKey(AudioKeys.PlayerPrefMusicVolume);
        musicSlider.value = resetMusicVolume;
        SetVolume(AudioKeys.MixerMusicParam, resetMusicVolume);
        UpdateMusicAmountText(resetMusicVolume);

        float linear = (resetMusicVolume > 1f) ? Mathf.Clamp01(resetMusicVolume / (musicSlider != null ? musicSlider.maxValue : 100f)) : Mathf.Clamp01(resetMusicVolume);
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(linear);
    }

    public void ResetSfx()
    {
        sfxSlider.value = resetSfxVolume;
        PlayerPrefs.DeleteKey(AudioKeys.PlayerPrefSfxVolume);
        sfxSlider.value = resetSfxVolume;
        SetVolume(AudioKeys.MixerSfxParam, resetSfxVolume);
        UpdateSfxAmountText(resetSfxVolume);
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

    private void OnLocaleChanged(Locale locale)
    {
        int qIdx = qualityLevelDropdown != null ? qualityLevelDropdown.value : 0;
        int sIdx = shadowResolutionDropdown != null ? shadowResolutionDropdown.value : 0;

        RefreshQualityDropdownLocalization();
        RefreshShadowDropdownLocalization();

        if (qualityLevelDropdown != null) qualityLevelDropdown.value = qIdx;
        if (shadowResolutionDropdown != null) shadowResolutionDropdown.value = sIdx;

        if (qualityLevelDropdown != null) qualityLevelDropdown.RefreshShownValue();
        if (shadowResolutionDropdown != null) shadowResolutionDropdown.RefreshShownValue();

        SyncLanguageDropdownWithSelectedLocale();
    }

    private void RefreshQualityDropdownLocalization()
    {
        if (qualityLevelDropdown == null) return;

        int selected = qualityLevelDropdown.value;

        qualityLevelDropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>();
        var names = QualitySettings.names;

        for (int i = 0; i < names.Length; i++)
        {
            string key = $"Settings_{names[i]}";
            string text = LocalizationSettings.StringDatabase.GetLocalizedString(uiStringTable, key);
            if (string.IsNullOrEmpty(text))
                text = names[i];

            options.Add(new TMP_Dropdown.OptionData(text));
        }

        qualityLevelDropdown.AddOptions(options);
        qualityLevelDropdown.value = Mathf.Clamp(selected, 0, options.Count - 1);
        qualityLevelDropdown.RefreshShownValue();
    }

    private void RefreshShadowDropdownLocalization()
    {
        if (shadowResolutionDropdown == null) return;

        int selected = shadowResolutionDropdown.value;

        shadowResolutionDropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var resolution in availableShadowResolutions)
        {
            string key = $"Settings_{resolution}";
            string text = LocalizationSettings.StringDatabase.GetLocalizedString(uiStringTable, key);
            if (string.IsNullOrEmpty(text))
                text = resolution.ToString();

            options.Add(new TMP_Dropdown.OptionData(text));
        }

        shadowResolutionDropdown.AddOptions(options);
        shadowResolutionDropdown.value = Mathf.Clamp(selected, 0, options.Count - 1);
        shadowResolutionDropdown.RefreshShownValue();
    }

    public void OnInvertMouseToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("InvertMouse", isOn ? 1 : 0);

        if (mouseLook != null)
            mouseLook.SetInvert(isOn);
    }

    private void ReadingInvertMouseSaveValue()
    {
        bool invert = PlayerPrefs.GetInt("InvertMouse", 0) == 1;

        if (invertMouseToggle != null)
            invertMouseToggle.isOn = invert;

        if (mouseLook != null)
            mouseLook.SetInvert(invert);
    }

    private void PersistSelectedLocale(Locale locale)
    {
        if (locale == null) return;

        LocalizationSettings.SelectedLocale = locale;

        PlayerPrefs.SetString(LocalePrefKey, locale.Identifier.Code);

        var selectors = LocalizationSettings.StartupLocaleSelectors;
        foreach (var s in selectors)
        {
            if (s is PlayerPrefLocaleSelector pp && !string.IsNullOrEmpty(pp.PlayerPreferenceKey))
            {
                PlayerPrefs.SetString(pp.PlayerPreferenceKey, locale.Identifier.Code);
                break;
            }
        }

        PlayerPrefs.Save();
    }
}
