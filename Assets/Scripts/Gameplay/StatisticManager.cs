using TMPro;
using UnityEngine;

public class StatisticManager : MonoBehaviour
{
    [Header("Statistics Data")]
    [SerializeField] private StatisticsData statisticsData;
    [Header("Title Statistics")]
    [SerializeField] private TextMeshProUGUI statistics;
    [Header("Statistics")]
    [SerializeField] private TextMeshProUGUI damageDeal;
    [SerializeField] private TextMeshProUGUI ammoShot;
    [SerializeField] private TextMeshProUGUI healthLost;
    [SerializeField] private TextMeshProUGUI healthHeal;
    [SerializeField] private TextMeshProUGUI oxygenLost;
    [SerializeField] private TextMeshProUGUI oxygenRecovery;
    [Header("Amount")]
    [SerializeField] private TextMeshProUGUI amountDamageDeal;
    [SerializeField] private TextMeshProUGUI amountAmmoShot;
    [SerializeField] private TextMeshProUGUI amountHealthLost;
    [SerializeField] private TextMeshProUGUI amountHealthHeal;
    [SerializeField] private TextMeshProUGUI amountOxygenLost;
    [SerializeField] private TextMeshProUGUI amountOxygenRecovery;
    [Header("Title Monter Name")]
    [SerializeField] private TextMeshProUGUI titleMonsterName;
    [Header("Monter Name")]
    [SerializeField] private TextMeshProUGUI alien;
    [SerializeField] private TextMeshProUGUI alienMag;
    [SerializeField] private TextMeshProUGUI orc;
    [SerializeField] private TextMeshProUGUI robot;
    [SerializeField] private TextMeshProUGUI Boss;
    [Header("Title Kill Amount")]
    [SerializeField] private TextMeshProUGUI titleKillAmount;
    [Header("Kill Amount")]
    [SerializeField] private TextMeshProUGUI alienAmount;
    [SerializeField] private TextMeshProUGUI alienMagAmount;
    [SerializeField] private TextMeshProUGUI orcAmount;
    [SerializeField] private TextMeshProUGUI robotAmount;
    [SerializeField] private TextMeshProUGUI bossAmount;
    [Header("Title Others")]
    [SerializeField] private TextMeshProUGUI titleOthers;
    [Header("Others")]
    [SerializeField] private TextMeshProUGUI allQuests;
    [SerializeField] private TextMeshProUGUI questGet;
    [SerializeField] private TextMeshProUGUI questCompleted;
    [SerializeField] private TextMeshProUGUI itemPickUp;
    [SerializeField] private TextMeshProUGUI itemUsed;
    [SerializeField] private TextMeshProUGUI itemUse;
    [SerializeField] private TextMeshProUGUI pickUpAmmo;
    [SerializeField] private TextMeshProUGUI grenadeUse;
    [SerializeField] private TextMeshProUGUI smokeGrenadeUse;
    [Header("Others Amount")]
    [SerializeField] private TextMeshProUGUI amountAllQuests;
    [SerializeField] private TextMeshProUGUI amountQuestGet;
    [SerializeField] private TextMeshProUGUI amountQuestCompleted;
    [SerializeField] private TextMeshProUGUI amountItemPickUp;
    [SerializeField] private TextMeshProUGUI amountItemUsed;
    [SerializeField] private TextMeshProUGUI amountPickUpAmmo;
    [SerializeField] private TextMeshProUGUI amountGrenadeUse;
    [SerializeField] private TextMeshProUGUI amountSmokeGrenadeUse;

    private void Start()
    {
        StatisticsCollector.SetStatisticsData(statisticsData);
        StatisticsCollector.OnStatsUpdated += GenerateStats;

        GenerateStats();
    }

    private void OnDestroy()
    {
        StatisticsCollector.OnStatsUpdated -= GenerateStats;
    }

    public void GenerateStats()
    {
        amountDamageDeal.text = statisticsData.damageDealt.ToString();
        amountAmmoShot.text = statisticsData.ammoShot.ToString();
        amountHealthLost.text = statisticsData.healthLost.ToString();
        amountHealthHeal.text = statisticsData.healthHealed.ToString();
        amountOxygenLost.text = statisticsData.oxygenLost.ToString();
        amountOxygenRecovery.text = statisticsData.oxygenRecovery.ToString();

        alienAmount.text = statisticsData.alienKillCount.ToString();
        alienMagAmount.text = statisticsData.alienMagKillCount.ToString();
        orcAmount.text = statisticsData.orcKillCount.ToString();
        robotAmount.text = statisticsData.robotKillCount.ToString();
        bossAmount.text = statisticsData.bossKillCount.ToString();

        amountAllQuests.text = statisticsData.allQuestsCount.ToString();
        amountQuestGet.text = statisticsData.questsGetCount.ToString();
        amountQuestCompleted.text = statisticsData.questsCompletedCount.ToString();
        amountItemPickUp.text = statisticsData.itemsPickedUpCount.ToString();
        amountItemUsed.text = statisticsData.itemsUsedCount.ToString();
        amountPickUpAmmo.text = statisticsData.ammoPickedUpCount.ToString();
        amountGrenadeUse.text = statisticsData.grenadesUsedCount.ToString();
        amountSmokeGrenadeUse.text = statisticsData.smokeGrenadesUsedCount.ToString();
    }
}
