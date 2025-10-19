using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
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
    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SfxVolume";
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

        SetSoundState(soundToggle.isOn);

        ReadingSoundsSaveValues();
        ReadingQualitySaveValues();
        ReadingResolutionSaveValues();
        ReadingAntiAliasingSaveValues();
        ReadingShadowSaveValues();
        ReadingToggleMuteSaveValue();
        ReadingToggleFullscreenSaveValue();

        ReadingInvertMouseSaveValue();
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

        // Zbieramy tylko PL i EN (jak dotychczas)
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

        // 1) Spróbuj odczytaæ kod z PlayerPrefLocaleSelector
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

        // 2) Jeœli brak, odczytaj nasz w³asny klucz
        if (string.IsNullOrEmpty(savedCode) && PlayerPrefs.HasKey(LocalePrefKey))
            savedCode = PlayerPrefs.GetString(LocalePrefKey);

        // 3) Jeœli nadal brak, u¿yj aktualnie ustawionego locale
        if (string.IsNullOrEmpty(savedCode) && LocalizationSettings.SelectedLocale != null)
            savedCode = LocalizationSettings.SelectedLocale.Identifier.Code;

        // Ustal docelowy locale i indeks dropdownu
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

        // Fallback: PL lub pierwszy dostêpny
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

        // Zapisz/persistuj tylko wybrany docelowy (nie nadpisuj zawsze na PL)
        if (targetLocale != null)
            PersistSelectedLocale(targetLocale);

        // Ustaw dropdown pod aktualny locale (bez wymuszania 0)
        languageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownValueChanged);
        languageDropdown.value = dropdownIndex;
        languageDropdown.RefreshShownValue();
        languageDropdown.onValueChanged.AddListener(OnLanguageDropdownValueChanged);

        // Synchronizuj dodatkowo, gdyby SelectedLocale ró¿ni³ siê od indexu
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
        float savedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", resetMasterVolume);
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", resetMusicVolume);
        float savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", resetSfxVolume);
        masterSlider.value = savedMasterVolume;
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSfxVolume;
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
        audioMainMixer.SetFloat(parameterName, Mathf.Log10(volume) * 20);
    }

    public void OnMasterVolumeChanged(float volume)
    {
        SetVolume(MIXER_MASTER, volume);
        UpdateMasterAmountText(volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        SetVolume(MIXER_MUSIC, volume);
        UpdateMusicAmountText(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        SetVolume(MIXER_SFX, volume);
        UpdateSfxAmountText(volume);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    public void OnSoundToggleChanged(bool isSoundOn)
    {
        SetSoundState(isSoundOn);
        PlayerPrefs.SetInt("ToggleMute", isSoundOn ? 1 : 0);
    }

    private void ReadingToggleMuteSaveValue()
    {
        if (PlayerPrefs.HasKey("ToggleMute"))
        {
            int savedState = PlayerPrefs.GetInt("ToggleMute");
            soundToggle.isOn = savedState == 1;
        }
    }

    public void ResetMute()
    {
        if (soundToggle != null)
        {
            PlayerPrefs.DeleteKey("ToggleMute");
            soundToggle.isOn = false;
        }
        else
        {
            Debug.LogError("Toggle nie jest przypisany. Przypisz Toggle w inspektorze.");
        }
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
            SetVolume(MIXER_MASTER, 0f);
            SetVolume(MIXER_MUSIC, 0f);
            SetVolume(MIXER_SFX, 0f);
        }
    }

    public void ResetMaster()
    {
        masterSlider.value = resetMasterVolume;
        PlayerPrefs.DeleteKey("MasterVolume");
        masterSlider.value = resetMasterVolume;
        SetVolume(MIXER_MASTER, resetMasterVolume);
        UpdateMasterAmountText(resetMasterVolume);
    }

    public void ResetMusic()
    {
        musicSlider.value = resetMusicVolume;
        PlayerPrefs.DeleteKey("MusicVolume");
        musicSlider.value = resetMusicVolume;
        SetVolume(MIXER_MUSIC, resetMusicVolume);
        UpdateMusicAmountText(resetMusicVolume);
    }

    public void ResetSfx()
    {
        sfxSlider.value = resetSfxVolume;
        PlayerPrefs.DeleteKey("SfxVolume");
        sfxSlider.value = resetSfxVolume;
        SetVolume(MIXER_SFX, resetSfxVolume);
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

    // ZMIANA: zawsze zapisuj PlayerPrefs i ustawiaj MouseLook
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
