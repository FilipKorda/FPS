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

}
