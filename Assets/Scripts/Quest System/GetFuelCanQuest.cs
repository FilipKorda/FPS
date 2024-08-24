using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Fuel Can Quest/Fuel Can Condition")]
public class GetFuelCanQuest : Quest
{
    public override bool CheckCompletion()
    {
        return MainInventory.Instance.fuelCanForQuest == 4;
    }
}
