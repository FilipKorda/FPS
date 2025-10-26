using UnityEngine;

[CreateAssetMenu(fileName = "Statistics Data", menuName = "Statistics/Main Statistic Data")]
public class StatisticsData : ScriptableObject
{
    [Header("-----------------------")]
    public float damageDealt;
    public int ammoShot;
    public float healthLost;
    public float healthHealed;
    public float oxygenLost;
    public float oxygenRecovery;
    [Header("-----------------------")]
    public int alienKillCount;
    public int alienMagKillCount;
    public int orcKillCount;
    public int robotKillCount;
    public int bossKillCount;
    [Header("-----------------------")]
    public int allQuestsCount;
    public int questsGetCount;
    public int questsCompletedCount;
    public int itemsPickedUpCount;
    public int itemsUsedCount;
    public int ammoPickedUpCount;
    public int grenadesUsedCount;
    public int smokeGrenadesUsedCount;

    [ContextMenu(" --- Reset Stats ---")]
    public void ResetStats()
    {
        damageDealt = 0f;
        ammoShot = 0;
        healthLost = 0f;
        healthHealed = 0f;
        oxygenLost = 0f;
        oxygenRecovery = 0f;

        alienKillCount = 0;
        alienMagKillCount = 0;
        orcKillCount = 0;
        robotKillCount = 0;
        bossKillCount = 0;

        allQuestsCount = 0;
        questsGetCount = 0;
        questsCompletedCount = 0;
        itemsPickedUpCount = 0;
        itemsUsedCount = 0;
        ammoPickedUpCount = 0;
        grenadesUsedCount = 0;
        smokeGrenadesUsedCount = 0;
    }

}
