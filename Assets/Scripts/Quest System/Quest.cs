using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    public bool isCompleted;

    public virtual bool CheckCompletion()
    {
        return isCompleted;
    }
}
