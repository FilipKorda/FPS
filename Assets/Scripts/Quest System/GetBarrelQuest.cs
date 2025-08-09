using UnityEngine;
[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Supplies/Barrel Condition")]
public class GetBarrelQuest : Quest
{
    public override bool CheckCompletion()
    {
        return MainInventory.Instance.currentBarrels == 1;
    }
}
