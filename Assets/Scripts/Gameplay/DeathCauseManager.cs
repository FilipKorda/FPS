using UnityEngine;
using UnityEngine.Localization;

public enum DeathCause
{
    Unknown = 0,
    Enemy1 = 1,
    Enemy2 = 2,
    Enemy3 = 3,
    Enemy4 = 4,
    Enemy5 = 5,
    Boss   = 6,
    NoOxygen = 7,
    NoMask   = 8
}

[DisallowMultipleComponent]
public class DeathCauseManager : MonoBehaviour
{
    public static DeathCauseManager Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    [Header("T³umaczenia (LocalizedString)")]
    [SerializeField] private LocalizedString unknownDeath;

    [Header("Wrogowie (5 typów)")]
    [SerializeField] private LocalizedString enemy1Death;
    [SerializeField] private LocalizedString enemy2Death;
    [SerializeField] private LocalizedString enemy3Death;
    [SerializeField] private LocalizedString enemy4Death;
    [SerializeField] private LocalizedString enemy5Death;
    
    [Header("Boss")]
    [SerializeField] private LocalizedString bossDeath;

    [Header("Œrodowisko")]
    [SerializeField] private LocalizedString noOxygenDeath;
    [SerializeField] private LocalizedString noMaskDeath;

    [SerializeField] private DeathCause current = DeathCause.Unknown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void ResetCause() => Instance?.ResetCurrent();
    public static void MarkKilledByEnemy(int enemyIndex) => Instance?.SetEnemyByIndex(enemyIndex);
    public static void MarkKilledByBoss() => Instance?.SetCause(DeathCause.Boss);
    public static void MarkNoOxygen() => Instance?.SetCause(DeathCause.NoOxygen);
    public static void MarkNoMask() => Instance?.SetCause(DeathCause.NoMask);

    public void ResetCurrent() => current = DeathCause.Unknown;
    public void SetEnemyByIndex(int enemyIndex)
    {
        enemyIndex = Mathf.Clamp(enemyIndex, 0, 4);
        current = (DeathCause)((int)DeathCause.Enemy1 + enemyIndex);
    }
    public void SetCause(DeathCause cause) => current = cause;

    public DeathCause GetCurrent() => current;

    public LocalizedString GetLocalizedStringAsset()
    {
        switch (current)
        {
            case DeathCause.Enemy1:   return enemy1Death;
            case DeathCause.Enemy2:   return enemy2Death;
            case DeathCause.Enemy3:   return enemy3Death;
            case DeathCause.Enemy4:   return enemy4Death;
            case DeathCause.Enemy5:   return enemy5Death;
            case DeathCause.Boss:     return bossDeath;
            case DeathCause.NoOxygen: return noOxygenDeath;
            case DeathCause.NoMask:   return noMaskDeath;
            case DeathCause.Unknown:
            default:                  return unknownDeath;
        }
    }

    public string GetDeathMessage()
    {
        var ls = GetLocalizedStringAsset();
        return ls != null ? ls.GetLocalizedString() : string.Empty;
    }
}