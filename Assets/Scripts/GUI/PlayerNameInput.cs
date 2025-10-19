using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

[DisallowMultipleComponent]
public class PlayerNameInput : MonoBehaviour
{
    public const string PlayerPrefsKey = "PlayerName";

    [Header("UI - Nazwa gracza")]
    [SerializeField] private TMP_InputField tmpInputField;   
    [SerializeField] private Button confirmButton;

    [Header("Ustawienia")]
    [SerializeField] private int maxLength = 16;
    [SerializeField] private bool restrictCharacters = true;

    [Header("UI - Mapowanie klawiszy na przyciski (q..m)")]
    [SerializeField] private Button[] keyButtons;

    [Header("UI - Caps/Shift/Backspace")]
    [SerializeField] private Button capsLockButton;
    [SerializeField] private Button shiftButton;
    [SerializeField] private Button backspaceButton;

    [SerializeField] private LocalizeStringEvent localizeString;

    private static readonly KeyCode[] KeyOrder = new KeyCode[]
    {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T,
        KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B,
        KeyCode.N, KeyCode.M
    };

    private TMP_Text[] _keyLabels;
    private UnityAction[] _buttonAppendActions;

    private bool _capsLock;
    private bool _shiftArmed; 

    private static string _cachedName;

    // Referencje placeholdera
    private Graphic _placeholderGraphic;
    private TMP_Text _placeholderText;

    // Cache stanu do ograniczenia zbêdnych operacji
    private bool _lastFocused;
    private bool _lastEmpty;

    public static event Action<string> OnPlayerNameChanged;

    public static string CurrentName
    {
        get
        {
            if (_cachedName == null)
            {
                _cachedName = PlayerPrefs.HasKey(PlayerPrefsKey)
                    ? PlayerPrefs.GetString(PlayerPrefsKey)
                    : string.Empty;
            }
            return _cachedName;
        }
    }

    private void Awake()
    {
        if (tmpInputField == null) tmpInputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable()
    {
        string saved = PlayerPrefs.HasKey(PlayerPrefsKey)
            ? PlayerPrefs.GetString(PlayerPrefsKey)
            : string.Empty;
        SetText(saved);
        ApplyCharacterLimit();

        CachePlaceholder();

        if (tmpInputField != null)
        {
            tmpInputField.onEndEdit.AddListener(OnEndEdit);
            tmpInputField.onSelect.AddListener(OnInputSelected);
            tmpInputField.onDeselect.AddListener(OnInputDeselected);
            tmpInputField.onValueChanged.AddListener(OnValueChanged);
        }

        if (confirmButton != null)
            confirmButton.onClick.AddListener(SaveFromUI);

        if (capsLockButton != null)
            capsLockButton.onClick.AddListener(ToggleCapsLock);

        if (shiftButton != null)
            shiftButton.onClick.AddListener(ArmShiftOnce);

        if (backspaceButton != null)
            backspaceButton.onClick.AddListener(BackspaceOnce);

        CacheKeyLabels();
        ApplyKeyLabels();
        BindKeyButtons();
        WarnIfCountMismatch();

        ForcePlaceholderState(); // inicjalne wymuszenie
    }

    private void OnDisable()
    {
        if (tmpInputField != null)
        {
            tmpInputField.onEndEdit.RemoveListener(OnEndEdit);
            tmpInputField.onSelect.RemoveListener(OnInputSelected);
            tmpInputField.onDeselect.RemoveListener(OnInputDeselected);
            tmpInputField.onValueChanged.RemoveListener(OnValueChanged);
        }

        if (confirmButton != null)
            confirmButton.onClick.RemoveListener(SaveFromUI);

        if (capsLockButton != null)
            capsLockButton.onClick.RemoveListener(ToggleCapsLock);

        if (shiftButton != null)
            shiftButton.onClick.RemoveListener(ArmShiftOnce);

        if (backspaceButton != null)
            backspaceButton.onClick.RemoveListener(BackspaceOnce);

        UnbindKeyButtons();
    }

    private void Update()
    {
        if (IsTextFieldFocused())
            return;

        int max = Mathf.Min(keyButtons != null ? keyButtons.Length : 0, KeyOrder.Length);
        for (int i = 0; i < max; i++)
        {
            if (Input.GetKeyDown(KeyOrder[i]))
            {
                InvokeKeyButton(i);
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            BackspaceOnce();
        }
    }

    // Wymuszaj stan placeholdera po wewnêtrznych aktualizacjach TMP
    private void LateUpdate()
    {
        ForcePlaceholderState();
    }

    private bool IsTextFieldFocused()
    {
        if (tmpInputField != null && tmpInputField.isFocused) return true;
        return false;
    }

    private void OnValidate()
    {
        ApplyCharacterLimit();
    }

    private void ApplyCharacterLimit()
    {
        int limit = maxLength > 0 ? maxLength : 0;
        if (tmpInputField != null) tmpInputField.characterLimit = limit;
    }

    private void OnEndEdit(string _)
    {
        SaveFromUI();
    }

    public void SaveFromUI()
    {
        string name = GetText()?.Trim() ?? string.Empty;

        if (restrictCharacters)
        {
            var sb = new System.Text.StringBuilder(name.Length);
            foreach (char c in name)
            {
                if (char.IsLetterOrDigit(c) || c == ' ' || c == '_' || c == '-')
                    sb.Append(c);
            }
            name = sb.ToString();
        }

        if (maxLength > 0 && name.Length > maxLength)
            name = name.Substring(0, maxLength);

        SetText(name);

        _cachedName = name;
        PlayerPrefs.SetString(PlayerPrefsKey, name);
        PlayerPrefs.Save();
        OnPlayerNameChanged?.Invoke(name);
    }

    private string GetText()
    {
        if (tmpInputField != null) return tmpInputField.text;
        return null;
    }

    private void SetText(string value)
    {
        if (tmpInputField != null) tmpInputField.text = value;
        // Stan placeholdera i tak zostanie wymuszony w LateUpdate,
        // ale aktualizujemy cache by ograniczyæ operacje.
        _lastEmpty = string.IsNullOrEmpty(value);
    }

    public static string GetSavedPlayerName(string fallback = "")
    {
        return PlayerPrefs.HasKey(PlayerPrefsKey) ? PlayerPrefs.GetString(PlayerPrefsKey) : fallback;
    }

    public static void SetSavedPlayerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        PlayerPrefs.SetString(PlayerPrefsKey, name);
        PlayerPrefs.Save();
        _cachedName = name;
        OnPlayerNameChanged?.Invoke(name);
    }

    private void ToggleCapsLock()
    {
        SetCapsLock(!_capsLock);
    }

    private void SetCapsLock(bool on)
    {
        if (_capsLock == on) return;
        _capsLock = on;
        ApplyKeyLabels();
    }

    private void ArmShiftOnce()
    {
        _shiftArmed = true;
        ApplyKeyLabels(); 
    }

    private void BackspaceOnce()
    {
        if (tmpInputField == null) return;
        string current = tmpInputField.text ?? string.Empty;
        if (current.Length == 0) return;

        tmpInputField.text = current.Substring(0, current.Length - 1);
        tmpInputField.caretPosition = tmpInputField.text.Length;
    }

    private void CacheKeyLabels()
    {
        if (keyButtons == null) return;

        int n = keyButtons.Length;
        _keyLabels = new TMP_Text[n];

        for (int i = 0; i < n; i++)
        {
            var btn = keyButtons[i];
            if (btn == null) continue;

            var labelUGUI = btn.GetComponentInChildren<TextMeshProUGUI>(true);
            TMP_Text label = labelUGUI != null ? labelUGUI : btn.GetComponentInChildren<TMP_Text>(true);
            _keyLabels[i] = label;
        }
    }

    private void ApplyKeyLabels()
    {
        if (keyButtons == null || _keyLabels == null) return;

        bool upper = _capsLock ^ _shiftArmed;
        int max = Mathf.Min(keyButtons.Length, KeyOrder.Length);
        for (int i = 0; i < max; i++)
        {
            var label = _keyLabels[i];
            if (label == null) continue;

            string ch = KeyOrder[i].ToString();
            label.text = upper ? ch.ToUpperInvariant() : ch.ToLowerInvariant();
        }
    }

    private void BindKeyButtons()
    {
        if (keyButtons == null) return;

        int max = Mathf.Min(keyButtons.Length, KeyOrder.Length);
        _buttonAppendActions = new UnityAction[keyButtons.Length];

        for (int i = 0; i < max; i++)
        {
            var btn = keyButtons[i];
            if (btn == null) continue;

            int idx = i; 
            UnityAction action = () => AppendKeyByIndex(idx);
            btn.onClick.AddListener(action);
            _buttonAppendActions[idx] = action;
        }
    }

    private void UnbindKeyButtons()
    {
        if (keyButtons == null || _buttonAppendActions == null) return;

        int n = Mathf.Min(keyButtons.Length, _buttonAppendActions.Length);
        for (int i = 0; i < n; i++)
        {
            if (keyButtons[i] != null && _buttonAppendActions[i] != null)
                keyButtons[i].onClick.RemoveListener(_buttonAppendActions[i]);
        }
        _buttonAppendActions = null;
    }

    private void AppendKeyByIndex(int index)
    {
        if (index < 0 || index >= KeyOrder.Length) return;

        string ch = KeyOrder[index].ToString();

        bool makeUpper = _capsLock ^ _shiftArmed;
        ch = makeUpper ? ch.ToUpperInvariant() : ch.ToLowerInvariant();

        AppendCharacter(ch);

        if (_shiftArmed)
        {
            _shiftArmed = false;
            ApplyKeyLabels();
        }
    }

    private void AppendCharacter(string ch)
    {
        if (string.IsNullOrEmpty(ch) || tmpInputField == null) return;

        string current = tmpInputField.text ?? string.Empty;

        if (restrictCharacters)
        {
            foreach (char c in ch)
            {
                if (!(char.IsLetterOrDigit(c) || c == ' ' || c == '_' || c == '-'))
                    return;
            }
        }

        if (maxLength > 0 && current.Length >= maxLength)
            return;

        string appended = current + ch;
        if (maxLength > 0 && appended.Length > maxLength)
            appended = appended.Substring(0, maxLength);

        tmpInputField.text = appended;
        tmpInputField.caretPosition = tmpInputField.text.Length;
    }

    private void InvokeKeyButton(int index)
    {
        if (keyButtons == null) return;
        if (index < 0 || index >= keyButtons.Length) return;

        var btn = keyButtons[index];
        if (btn != null && btn.IsActive() && btn.interactable)
            btn.onClick?.Invoke();
    }

    private void WarnIfCountMismatch()
    {
        if (keyButtons == null) return;
        if (keyButtons.Length != KeyOrder.Length)
        {
            Debug.LogWarning($"PlayerNameInput: liczba przycisków ({keyButtons.Length}) ró¿ni siê od liczby klawiszy ({KeyOrder.Length}).");
        }
    }

    public void RefreshKeyLabels()
    {
        CacheKeyLabels();
        ApplyKeyLabels();
        UnbindKeyButtons();
        BindKeyButtons();
    }

    // --- Placeholder helpers ---

    private void CachePlaceholder()
    {
        if (tmpInputField == null) return;
        _placeholderGraphic = tmpInputField.placeholder;
        _placeholderText = _placeholderGraphic != null ? _placeholderGraphic.GetComponent<TextMeshProUGUI>() : null;

        if (localizeString == null && _placeholderText != null)
            localizeString = _placeholderText.GetComponent<LocalizeStringEvent>();

        // Zainicjalizuj cache stanu
        _lastFocused = tmpInputField.isFocused;
        _lastEmpty = string.IsNullOrEmpty(tmpInputField.text);
    }

    private void OnInputSelected(string _)
    {
        // Nic nie odpinamy — stan zostanie wymuszony w LateUpdate
    }

    private void OnInputDeselected(string _)
    {
        // Nic nie odpinamy — stan zostanie wymuszony w LateUpdate
    }

    private void OnValueChanged(string _)
    {
        // Aktualizacja nast¹pi w LateUpdate; tu tylko szybka aktualizacja cache
        _lastEmpty = string.IsNullOrEmpty(tmpInputField.text);
    }

    private void ForcePlaceholderState()
    {
        if (tmpInputField == null || _placeholderGraphic == null) return;

        bool focused = tmpInputField.isFocused;
        bool empty = string.IsNullOrEmpty(tmpInputField.text);

        // Tylko jeœli stan siê zmieni³, wykonaj operacje
        if (focused != _lastFocused || empty != _lastEmpty || _placeholderGraphic.gameObject.activeSelf == focused || (!focused && _placeholderGraphic.gameObject.activeSelf != empty))
        {
            bool shouldShow = !focused && empty;

            // Upewnij siê, ¿e tmpInputField.placeholder wskazuje na nasz placeholder
            if (tmpInputField.placeholder == null)
                tmpInputField.placeholder = _placeholderGraphic;

            if (_placeholderGraphic.gameObject.activeSelf != shouldShow)
                _placeholderGraphic.gameObject.SetActive(shouldShow);

            if (shouldShow && localizeString != null)
                localizeString.RefreshString();

            _lastFocused = focused;
            _lastEmpty = empty;
        }
    }
}