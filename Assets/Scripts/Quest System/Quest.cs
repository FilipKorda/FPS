using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public LocalizedString localizeQuestName;
    public LocalizedString localizeQuestDescription;

    public string questName;
    public string questDescription;
    public bool isCompleted;

    public virtual bool CheckCompletion()
    {
        return isCompleted;
    }
}
