using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    [Header("Buttons")]
    [SerializeField] private GameObject buttonHolder;


    private Vector3 targetStatisticsPosition;
    private Vector3 targetDamageDealPosition;
    private Vector3 targetAmmoShotPosition;
    private Vector3 targetHealthLostPosition;
    private Vector3 targetHealthHealPosition;
    private Vector3 targetOxygenLostPosition;
    private Vector3 targetOxygenRecoveryPosition;

    private Vector3 titleMonsterNamePosition;
    private Vector3 titleKillAmountPosition;
    private Vector3 targetAlienPosition;
    private Vector3 targetAlienMagPosition;
    private Vector3 targetOrcPosition;
    private Vector3 targetRobotPosition;
    private Vector3 targetBossPosition;

    private Vector3 titleOthersPosition;

    private Vector3 targetAllQuestsPosition;
    private Vector3 targetQuestGetPosition;
    private Vector3 targetQuestCompletedPosition;
    private Vector3 targetItemPickUpPosition;
    private Vector3 targetItemUsedPosition;
    private Vector3 targetPickUpAmmoPosition;
    private Vector3 targetGrenadeUsePosition;
    private Vector3 targetSmokeGrenadeUsePosition;

    private void Start()
    {
        if (statistics != null) targetStatisticsPosition = statistics.rectTransform.localPosition;
        if (titleMonsterName != null) titleMonsterNamePosition = titleMonsterName.rectTransform.localPosition;
        if (titleKillAmount != null) titleKillAmountPosition = titleKillAmount.rectTransform.localPosition;
        if (titleOthersPosition != null) titleOthersPosition = titleOthers.rectTransform.localPosition;

        if (damageDeal != null) targetDamageDealPosition = damageDeal.rectTransform.localPosition;
        if (ammoShot != null) targetAmmoShotPosition = ammoShot.rectTransform.localPosition;
        if (healthLost != null) targetHealthLostPosition = healthLost.rectTransform.localPosition;
        if (healthHeal != null) targetHealthHealPosition = healthHeal.rectTransform.localPosition;
        if (oxygenLost != null) targetOxygenLostPosition = oxygenLost.rectTransform.localPosition;
        if (oxygenRecovery != null) targetOxygenRecoveryPosition = oxygenRecovery.rectTransform.localPosition;

        if (alien != null) targetAlienPosition = alien.rectTransform.localPosition;
        if (alienMag != null) targetAlienMagPosition = alienMag.rectTransform.localPosition;
        if (orc != null) targetOrcPosition = orc.rectTransform.localPosition;
        if (robot != null) targetRobotPosition = robot.rectTransform.localPosition;
        if (Boss != null) targetBossPosition = Boss.rectTransform.localPosition;

        if (allQuests != null) targetAllQuestsPosition = allQuests.rectTransform.localPosition;
        if (questGet != null) targetQuestGetPosition = questGet.rectTransform.localPosition;
        if (questCompleted != null) targetQuestCompletedPosition = questCompleted.rectTransform.localPosition;
        if (itemPickUp != null) targetItemPickUpPosition = itemPickUp.rectTransform.localPosition;
        if (itemUsed != null) targetItemUsedPosition = itemUsed.rectTransform.localPosition;
        if (pickUpAmmo != null) targetPickUpAmmoPosition = pickUpAmmo.rectTransform.localPosition;
        if (grenadeUse != null) targetGrenadeUsePosition = grenadeUse.rectTransform.localPosition;
        if (smokeGrenadeUse != null) targetSmokeGrenadeUsePosition = smokeGrenadeUse.rectTransform.localPosition;

        Vector3 offset = Vector3.right * 200f;

        if (statistics != null) statistics.rectTransform.localPosition = targetStatisticsPosition + offset;
        if (titleMonsterName != null) titleMonsterName.rectTransform.localPosition = titleMonsterNamePosition + offset;
        if (titleKillAmount != null) titleKillAmount.rectTransform.localPosition = titleKillAmountPosition + offset;
        if (titleOthers != null) titleOthers.rectTransform.localPosition = titleOthersPosition + offset;

        if (damageDeal != null) damageDeal.rectTransform.localPosition = targetDamageDealPosition + offset;
        if (ammoShot != null) ammoShot.rectTransform.localPosition = targetAmmoShotPosition + offset;
        if (healthLost != null) healthLost.rectTransform.localPosition = targetHealthLostPosition + offset;
        if (healthHeal != null) healthHeal.rectTransform.localPosition = targetHealthHealPosition + offset;
        if (oxygenLost != null) oxygenLost.rectTransform.localPosition = targetOxygenLostPosition + offset;
        if (oxygenRecovery != null) oxygenRecovery.rectTransform.localPosition = targetOxygenRecoveryPosition + offset;

        if (alien != null) alien.rectTransform.localPosition = targetAlienPosition + offset;
        if (alienMag != null) alienMag.rectTransform.localPosition = targetAlienMagPosition + offset;
        if (orc != null) orc.rectTransform.localPosition = targetOrcPosition + offset;
        if (robot != null) robot.rectTransform.localPosition = targetRobotPosition + offset;
        if (Boss != null) Boss.rectTransform.localPosition = targetBossPosition + offset;

        if (allQuests != null) allQuests.rectTransform.localPosition = targetAllQuestsPosition + offset;
        if (questGet != null) questGet.rectTransform.localPosition = targetQuestGetPosition + offset;
        if (questCompleted != null) questCompleted.rectTransform.localPosition = targetQuestCompletedPosition + offset;
        if (itemPickUp != null) itemPickUp.rectTransform.localPosition = targetItemPickUpPosition + offset;
        if (itemUsed != null) itemUsed.rectTransform.localPosition = targetItemUsedPosition + offset;
        if (pickUpAmmo != null) pickUpAmmo.rectTransform.localPosition = targetPickUpAmmoPosition + offset;
        if (grenadeUse != null) grenadeUse.rectTransform.localPosition = targetGrenadeUsePosition + offset;
        if (smokeGrenadeUse != null) smokeGrenadeUse.rectTransform.localPosition = targetSmokeGrenadeUsePosition + offset;

        SetVertexGradientAlpha(statistics, 0f);

        SetVertexGradientAlpha(damageDeal, 0f);
        SetVertexGradientAlpha(ammoShot, 0f);
        SetVertexGradientAlpha(healthLost, 0f);
        SetVertexGradientAlpha(healthHeal, 0f);
        SetVertexGradientAlpha(oxygenLost, 0f);
        SetVertexGradientAlpha(oxygenRecovery, 0f);

        SetVertexGradientAlpha(amountDamageDeal, 0f);
        SetVertexGradientAlpha(amountAmmoShot, 0f);
        SetVertexGradientAlpha(amountHealthLost, 0f);
        SetVertexGradientAlpha(amountHealthHeal, 0f);
        SetVertexGradientAlpha(amountOxygenLost, 0f);
        SetVertexGradientAlpha(amountOxygenRecovery, 0f);

        SetVertexGradientAlpha(titleMonsterName, 0f);
        SetVertexGradientAlpha(alien, 0f);
        SetVertexGradientAlpha(alienMag, 0f);
        SetVertexGradientAlpha(orc, 0f);
        SetVertexGradientAlpha(robot, 0f);
        SetVertexGradientAlpha(Boss, 0f);

        SetVertexGradientAlpha(titleKillAmount, 0f);
        SetVertexGradientAlpha(alienAmount, 0f);
        SetVertexGradientAlpha(alienMagAmount, 0f);
        SetVertexGradientAlpha(orcAmount, 0f);
        SetVertexGradientAlpha(robotAmount, 0f);
        SetVertexGradientAlpha(bossAmount, 0f);

        SetVertexGradientAlpha(titleOthers, 0f);
        SetVertexGradientAlpha(allQuests, 0f);
        SetVertexGradientAlpha(questGet, 0f);
        SetVertexGradientAlpha(questCompleted, 0f);
        SetVertexGradientAlpha(itemPickUp, 0f);
        SetVertexGradientAlpha(itemUsed, 0f);
        SetVertexGradientAlpha(pickUpAmmo, 0f);
        SetVertexGradientAlpha(grenadeUse, 0f);
        SetVertexGradientAlpha(smokeGrenadeUse, 0f);

        SetVertexGradientAlpha(amountAllQuests, 0f);
        SetVertexGradientAlpha(amountQuestGet, 0f);
        SetVertexGradientAlpha(amountQuestCompleted, 0f);
        SetVertexGradientAlpha(amountItemPickUp, 0f);
        SetVertexGradientAlpha(amountItemUsed, 0f);
        SetVertexGradientAlpha(amountPickUpAmmo, 0f);
        SetVertexGradientAlpha(amountGrenadeUse, 0f);
        SetVertexGradientAlpha(amountSmokeGrenadeUse, 0f);

        buttonHolder.SetActive(false);
    }

    private void OnEnable()
    {
        if (statisticsData != null)
            StatisticsCollector.SetStatisticsData(statisticsData);

        StatisticsCollector.OnStatsUpdated += GenerateStats;
    }

    private void OnDisable()
    {
        StatisticsCollector.OnStatsUpdated -= GenerateStats;
    }

    private void OnDestroy()
    {
        StatisticsCollector.OnStatsUpdated -= GenerateStats;
    }

    [ContextMenu("Anim")]
    public void PlayEntryAnimation()
    {
        StartCoroutine(AnimateSequentially(0.1f, 0.2f));
    }


    private IEnumerator AnimateSequentially(float durationPerItem, float delayBetweenItems)
    {
        var sequence = new (RectTransform rt, TextMeshProUGUI tmp, Vector3 target)[]
        {
            // Sekwencja zgodnie z Twoj¹ kolejnoœci¹: label, then value
            (statistics   != null ? statistics.rectTransform   : null, statistics,   targetStatisticsPosition),

            (damageDeal    != null ? damageDeal.rectTransform    : null, damageDeal,    targetDamageDealPosition),
            (amountDamageDeal != null ? amountDamageDeal.rectTransform : null, amountDamageDeal, amountDamageDeal != null ? amountDamageDeal.rectTransform.localPosition : Vector3.zero),

            (ammoShot      != null ? ammoShot.rectTransform      : null, ammoShot,      targetAmmoShotPosition),
            (amountAmmoShot   != null ? amountAmmoShot.rectTransform   : null, amountAmmoShot,   amountAmmoShot   != null ? amountAmmoShot.rectTransform.localPosition   : Vector3.zero),

            (healthLost    != null ? healthLost.rectTransform    : null, healthLost,    targetHealthLostPosition),
            (amountHealthLost != null ? amountHealthLost.rectTransform : null, amountHealthLost, amountHealthLost != null ? amountHealthLost.rectTransform.localPosition : Vector3.zero),

            (healthHeal    != null ? healthHeal.rectTransform    : null, healthHeal,    targetHealthHealPosition),
            (amountHealthHeal != null ? amountHealthHeal.rectTransform : null, amountHealthHeal, amountHealthHeal != null ? amountHealthHeal.rectTransform.localPosition : Vector3.zero),

            (oxygenLost    != null ? oxygenLost.rectTransform    : null, oxygenLost,    targetOxygenLostPosition),
            (amountOxygenLost != null ? amountOxygenLost.rectTransform : null, amountOxygenLost, amountOxygenLost != null ? amountOxygenLost.rectTransform.localPosition : Vector3.zero),

            (oxygenRecovery!= null ? oxygenRecovery.rectTransform: null, oxygenRecovery,targetOxygenRecoveryPosition),
            (amountOxygenRecovery != null ? amountOxygenRecovery.rectTransform : null, amountOxygenRecovery, amountOxygenRecovery != null ? amountOxygenRecovery.rectTransform.localPosition : Vector3.zero),

            // Monster / kills (label then amounts)
            (titleMonsterName != null ? titleMonsterName.rectTransform : null, titleMonsterName, titleMonsterNamePosition),

            (titleKillAmount != null ? titleKillAmount.rectTransform : null, titleKillAmount, titleKillAmountPosition),

            (alien         != null ? alien.rectTransform         : null, alien,         targetAlienPosition),
            (alienAmount    != null ? alienAmount.rectTransform    : null, alienAmount,    alienAmount    != null ? alienAmount.rectTransform.localPosition    : Vector3.zero),

            (alienMag      != null ? alienMag.rectTransform      : null, alienMag,      targetAlienMagPosition),
            (alienMagAmount != null ? alienMagAmount.rectTransform : null, alienMagAmount, alienMagAmount != null ? alienMagAmount.rectTransform.localPosition : Vector3.zero),

            (orc           != null ? orc.rectTransform           : null, orc,           targetOrcPosition),
            (orcAmount      != null ? orcAmount.rectTransform      : null, orcAmount,      orcAmount      != null ? orcAmount.rectTransform.localPosition      : Vector3.zero),

            (robot         != null ? robot.rectTransform         : null, robot,         targetRobotPosition),
            (robotAmount    != null ? robotAmount.rectTransform    : null, robotAmount,    robotAmount    != null ? robotAmount.rectTransform.localPosition    : Vector3.zero),

            (Boss          != null ? Boss.rectTransform          : null, Boss,          targetBossPosition),
            (bossAmount     != null ? bossAmount.rectTransform     : null, bossAmount,     bossAmount     != null ? bossAmount.rectTransform.localPosition     : Vector3.zero),

            // Others (title then label/value pairs)
            (titleOthers   != null ? titleOthers.rectTransform   : null, titleOthers,   titleOthersPosition),

            (allQuests      != null ? allQuests.rectTransform      : null, allQuests,      targetAllQuestsPosition),
            (amountAllQuests      != null ? amountAllQuests.rectTransform      : null, amountAllQuests,      amountAllQuests      != null ? amountAllQuests.rectTransform.localPosition      : Vector3.zero),

            (questGet       != null ? questGet.rectTransform       : null, questGet,       targetQuestGetPosition),
            (amountQuestGet       != null ? amountQuestGet.rectTransform       : null, amountQuestGet,       amountQuestGet       != null ? amountQuestGet.rectTransform.localPosition       : Vector3.zero),

            (questCompleted != null ? questCompleted.rectTransform : null, questCompleted, targetQuestCompletedPosition),
            (amountQuestCompleted!= null ? amountQuestCompleted.rectTransform: null, amountQuestCompleted,amountQuestCompleted!= null ? amountQuestCompleted.rectTransform.localPosition: Vector3.zero),

            (itemPickUp     != null ? itemPickUp.rectTransform     : null, itemPickUp,     targetItemPickUpPosition),
            (amountItemPickUp     != null ? amountItemPickUp.rectTransform     : null, amountItemPickUp,     amountItemPickUp     != null ? amountItemPickUp.rectTransform.localPosition     : Vector3.zero),

            (itemUsed       != null ? itemUsed.rectTransform       : null, itemUsed,       targetItemUsedPosition),
            (amountItemUsed       != null ? amountItemUsed.rectTransform       : null, amountItemUsed,       amountItemUsed       != null ? amountItemUsed.rectTransform.localPosition       : Vector3.zero),

            (pickUpAmmo     != null ? pickUpAmmo.rectTransform     : null, pickUpAmmo,     targetPickUpAmmoPosition),
            (amountPickUpAmmo     != null ? amountPickUpAmmo.rectTransform     : null, amountPickUpAmmo,     amountPickUpAmmo     != null ? amountPickUpAmmo.rectTransform.localPosition     : Vector3.zero),

            (grenadeUse     != null ? grenadeUse.rectTransform     : null, grenadeUse,     targetGrenadeUsePosition),
            (amountGrenadeUse     != null ? amountGrenadeUse.rectTransform     : null, amountGrenadeUse,     amountGrenadeUse     != null ? amountGrenadeUse.rectTransform.localPosition     : Vector3.zero),

            (smokeGrenadeUse != null ? smokeGrenadeUse.rectTransform: null, smokeGrenadeUse, targetSmokeGrenadeUsePosition),
            (amountSmokeGrenadeUse != null ? amountSmokeGrenadeUse.rectTransform: null, amountSmokeGrenadeUse,amountSmokeGrenadeUse!= null ? amountSmokeGrenadeUse.rectTransform.localPosition: Vector3.zero),
        };

        foreach (var anim in sequence)
        {
            if (anim.rt == null && anim.tmp == null)
            {
                continue;
            }

            Vector3 from = anim.rt != null ? anim.rt.localPosition : Vector3.zero;
            Vector3 to = anim.target;

            float elapsed = 0f;
            while (elapsed < durationPerItem)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / durationPerItem);
                float e = Mathf.SmoothStep(0f, 1f, t);

                if (anim.rt != null)
                {
                    anim.rt.localPosition = Vector3.LerpUnclamped(from, to, e);
                }

                if (anim.tmp != null)
                {
                    SetVertexGradientAlpha(anim.tmp, e);
                }

                yield return null;
            }

            if (anim.rt != null) anim.rt.localPosition = to;
            if (anim.tmp != null) SetVertexGradientAlpha(anim.tmp, 1f);

            yield return new WaitForSecondsRealtime(delayBetweenItems);
        }

        buttonHolder.SetActive(true);
    }

    private void SetVertexGradientAlpha(TextMeshProUGUI tmp, float alpha)
    {
        if (tmp == null) return;

        tmp.alpha = Mathf.Clamp01(alpha);
        tmp.UpdateVertexData();
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
