using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private GameObject scrollViewContext;
    [SerializeField] private GameObject questHolderPrefab;

    [SerializeField] private List<Quest> questsConditions;
    [SerializeField] private List<Quest> questHolder;

    private Dictionary<Quest, GameObject> questToPrefabMap = new Dictionary<Quest, GameObject>();

    [SerializeField] private GetFixTurretQuest getFixTurretQuest;
    [SerializeField] private GetBackpackQuest getBackpackQuest;

    public LocalizedString localizeStringEvent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        UpdateStatistics();
    }

    private void UpdateStatistics()
    {
        int count = questHolder.Count;
        StatisticsCollector.AddAllQuests(count);
    }


    private void Update()
    {
        foreach (var quest in questsConditions)
        {
            if (!quest.isCompleted && quest.CheckCompletion())
            {
                quest.isCompleted = true;

                var localizedName = quest.localizeQuestName != null
                    ? quest.localizeQuestName.GetLocalizedString()
                    : quest.questName;

                Debug.Log($"{localizedName} completed!");

                RemoveCompletedQuest(quest);

                UpdateUI();
            }
        }
    }

    private void UpdateUI()
    {
        foreach (Transform child in scrollViewContext.transform)
        {
            Destroy(child.gameObject);
        }

        questToPrefabMap.Clear();

        foreach (var quest in questsConditions)
        {
            if (!quest.isCompleted)
            {
                AddQuestHolderPrefab(quest);
            }
        }
    }

    public void ResetAllCompletionQuest()
    {
        Debug.Log("<color=yellow>All tasks have been successfully reset!</color>");
        foreach (var quest in questHolder)
        {
            quest.isCompleted = false;

            questToPrefabMap.Clear();
        }
        getFixTurretQuest.isBarrelSet = false;
        getBackpackQuest.isBackpackSet = false;
        UpdateUI();
    }

    public void GetQuest(Quest newQuest)
    {
        if (!questsConditions.Contains(newQuest))
        {
            var localizedName = newQuest.localizeQuestName != null
                ? newQuest.localizeQuestName.GetLocalizedString()
                : newQuest.questName;

            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "<color=orange>Get Quest </color>" + localizedName, 2f, localizedName);

            questsConditions.Add(newQuest);
            Debug.Log($"New quest received: {localizedName}");
            UpdateUI();

            StatisticsCollector.AddQuestGet();
        }
    }

    private void AddQuestHolderPrefab(Quest quest)
    {
        GameObject instantiatedQuestHolderPrefab = Instantiate(questHolderPrefab, scrollViewContext.transform);

        TextMeshProUGUI questHolderQuestName = instantiatedQuestHolderPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI questHolderQuestDescription = instantiatedQuestHolderPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        questHolderQuestName.text = quest.localizeQuestName != null
            ? quest.localizeQuestName.GetLocalizedString()
            : quest.questName;

        questHolderQuestDescription.text = quest.localizeQuestDescription != null
            ? quest.localizeQuestDescription.GetLocalizedString()
            : quest.questDescription;

        questToPrefabMap.Add(quest, instantiatedQuestHolderPrefab);
    }

    private void RemoveCompletedQuest(Quest quest)
    {
        if (questToPrefabMap.ContainsKey(quest))
        {
            Destroy(questToPrefabMap[quest]);
            questToPrefabMap.Remove(quest);
        }

        StatisticsCollector.AddQuestCompleted();
    }
}
