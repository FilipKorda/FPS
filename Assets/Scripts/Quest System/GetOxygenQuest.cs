using UnityEngine;
[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Supplies/Oxygen Condition")]
public class GetOxygenQuest : Quest
{
    public override bool CheckCompletion()
    {
        return MainInventory.Instance.currentOxygenContainer == 4;
    }
}
