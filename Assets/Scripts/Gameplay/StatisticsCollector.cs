using System;

public enum EnemyKillType
{
    Alien,
    AlienMag,
    Orc,
    Robot,
    Boss
}

public static class StatisticsCollector
{
    private static StatisticsData data;

    public static event Action OnStatsUpdated;

    public static void SetStatisticsData(StatisticsData statisticsData)
    {
        data = statisticsData;
        OnStatsUpdated?.Invoke();
    }

    public static void Reset()
    {
        data = null;
        OnStatsUpdated?.Invoke();
    }

    public static void AddDamage(float amount)
    {
        if (data == null) return;
        data.damageDealt += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddAmmoShot(int count = 1)
    {
        if (data == null) return;
        data.ammoShot += count;
        OnStatsUpdated?.Invoke();
    }

    public static void AddHealthLost(float amount)
    {
        if (data == null) return;
        data.healthLost += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddHealthHealed(float amount)
    {
        if (data == null) return;
        data.healthHealed += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddOxygenLost(float amount)
    {
        if (data == null) return;
        data.oxygenLost += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddOxygenRecovery(float amount)
    {
        if (data == null) return;
        data.oxygenRecovery += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void IncrementKill(EnemyKillType type, int amount = 1)
    {
        if (data == null) return;
        switch (type)
        {
            case EnemyKillType.Alien:
                data.alienKillCount += amount;
                break;
            case EnemyKillType.AlienMag:
                data.alienMagKillCount += amount;
                break;
            case EnemyKillType.Orc:
                data.orcKillCount += amount;
                break;
            case EnemyKillType.Robot:
                data.robotKillCount += amount;
                break;
            case EnemyKillType.Boss:
                data.bossKillCount += amount;
                break;
        }
        OnStatsUpdated?.Invoke();
    }

    public static void AddQuestGet(int amount = 1)
    {
        if (data == null) return;
        data.questsGetCount += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddQuestCompleted(int amount = 1)
    {
        if (data == null) return;
        data.questsCompletedCount += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddAllQuests(int amount = 1)
    {
        if (data == null) return;
        data.allQuestsCount += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddItemPickedUp(int amount = 1)
    {
        if (data == null) return;
        data.itemsPickedUpCount += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddItemUsed(int amount = 1)
    {
        if (data == null) return;
        data.itemsUsedCount += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddAmmoPickedUp(int amount = 1)
    {
        if (data == null) return;
        data.ammoPickedUpCount += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddGrenadeUsed(int amount = 1)
    {
        if (data == null) return;
        data.grenadesUsedCount += amount;
        OnStatsUpdated?.Invoke();
    }

    public static void AddSmokeGrenadeUsed(int amount = 1)
    {
        if (data == null) return;
        data.smokeGrenadesUsedCount += amount;
        OnStatsUpdated?.Invoke();
    }
}