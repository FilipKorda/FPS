using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNameRuntime : MonoBehaviour
{
    public static PlayerNameRuntime Instance { get; private set; }

    [Tooltip("Imie musi byæ You bo to You sie podmienia na nzwe w inputFieldzie")]
    private string placeholderToken = "You";
    private string placeholderNewCharacterName = "";
    [SerializeField] private bool caseInsensitive = true;

    [Header("Sceny")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [SerializeField] private Conversation[] conversations;

    private readonly HashSet<ConversationData> _placeholderEntries = new HashSet<ConversationData>();
    public string PlayerName { get; private set; } = string.Empty;

    private bool _inMainMenu;

    private StringComparison Comparison => caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerName = PlayerNameInput.GetSavedPlayerName(string.Empty);
        placeholderNewCharacterName = PlayerName;

        PlayerNameInput.OnPlayerNameChanged += OnPlayerNameChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        ResetConversationsToPlaceholder();

        if (Instance == this)
            Instance = null;

        PlayerNameInput.OnPlayerNameChanged -= OnPlayerNameChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;

        _placeholderEntries.Clear();
    }

    private void OnApplicationQuit()
    {
        ResetConversationsToPlaceholder();
    }

    private void Start()
    {
        HandleScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleScene(scene);
    }

    private void HandleScene(Scene scene)
    {
        _inMainMenu = string.Equals(scene.name, mainMenuSceneName, StringComparison.Ordinal);

        if (_inMainMenu)
        {
            ResetConversationsToPlaceholder();
        }
        else
        {
            ApplyToAllLoadedConversations();
        }
    }

    private void OnPlayerNameChanged(string newName)
    {
        PlayerName = newName ?? string.Empty;
        placeholderNewCharacterName = PlayerName;

        if (_inMainMenu) return;

        foreach (var entry in _placeholderEntries)
        {
            if (entry != null)
                entry.Name = PlayerName;
        }
    }

    public string ResolveName(string rawName)
    {
        if (string.IsNullOrEmpty(rawName)) return rawName;
        return IsPlaceholder(rawName) ? PlayerName : rawName;
    }

    public void ApplyToConversation(Conversation conv)
    {
        if (conv == null || conv.conversation == null) return;

        foreach (var data in conv.conversation)
        {
            if (data == null) continue;

            if (IsPlaceholder(data.Name))
                _placeholderEntries.Add(data);

            if (!_inMainMenu && _placeholderEntries.Contains(data))
                data.Name = PlayerName;
        }
    }

    public void ApplyToAllLoadedConversations()
    {
        if (conversations != null)
        {
            foreach (var conv in conversations)
                ApplyToConversation(conv);
        }
    }

    public void ResetConversationsToPlaceholder()
    {
        foreach (var entry in _placeholderEntries)
        {
            if (entry != null)
                entry.Name = placeholderToken; 
        }
    }

    private bool IsPlaceholder(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        return string.Equals(name.Trim(), placeholderToken, Comparison);
    }
}