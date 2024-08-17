using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
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
        foreach (var quest in questsConditions)
        {
            if (!quest.isCompleted)
            {
                questName.text = quest.questName;
                questDescription.text = quest.questDescription;
                break;
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

}
