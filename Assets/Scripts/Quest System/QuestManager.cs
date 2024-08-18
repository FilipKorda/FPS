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

        foreach (var quest in questsConditions)
        {
            if (!quest.isCompleted)
            {
                AddQuestHolderPrefab(quest.questName, quest.questDescription);
            }
        }
    }

    public void ResetAllCompletionQuest()
    {
        Debug.Log("<color=yellow>All tasks have been successfully reset!</color>");
        foreach (var quest in questHolder)
        {
            quest.isCompleted = false;
        }
    }

    public void GetQuest(Quest newQuest)
    {
        if (!questsConditions.Contains(newQuest))
        {
            questsConditions.Add(newQuest);
            Debug.Log($"New quest received: {newQuest.questName}");
            UpdateUI();
        }
    }

    private void AddQuestHolderPrefab(string questName, string questDescription)
    {
        GameObject instantiatedQuestHolderPrefab = Instantiate(questHolderPrefab, scrollViewContext.transform);

        TextMeshProUGUI questHolderQuestName = instantiatedQuestHolderPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI questHolderQuestDescription = instantiatedQuestHolderPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        questHolderQuestName.text = questName;
        questHolderQuestDescription.text = questDescription;
    }
}
