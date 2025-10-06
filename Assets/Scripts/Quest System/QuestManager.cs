using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        foreach (var quest in questsConditions)
        {
            if (!quest.isCompleted && quest.CheckCompletion())
            {
                quest.isCompleted = true;
                Debug.Log($"{quest.questName} completed!");

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
            NotificationSystem.Instance.ShowNotification("<color=orange>Get Quest </color>" + newQuest.questName, 2f);
            questsConditions.Add(newQuest);
            Debug.Log($"New quest received: {newQuest.questName}");
            UpdateUI();
        }
    }

    private void AddQuestHolderPrefab(Quest quest)
    {
        GameObject instantiatedQuestHolderPrefab = Instantiate(questHolderPrefab, scrollViewContext.transform);

        TextMeshProUGUI questHolderQuestName = instantiatedQuestHolderPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI questHolderQuestDescription = instantiatedQuestHolderPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        questHolderQuestName.text = quest.questName;
        questHolderQuestDescription.text = quest.questDescription;

        questToPrefabMap.Add(quest, instantiatedQuestHolderPrefab);
    }

    private void RemoveCompletedQuest(Quest quest)
    {
        if (questToPrefabMap.ContainsKey(quest))
        {
            Destroy(questToPrefabMap[quest]);
            questToPrefabMap.Remove(quest);
        }
    }
}
