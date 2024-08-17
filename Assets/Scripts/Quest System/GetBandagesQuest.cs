
using UnityEngine;
[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Supplies/Bandages Condition")]
public class GetBandagesQuest : Quest
{
    public override bool CheckCompletion()
    {
        return MainInventory.Instance.currentHealthBandage == 4;
    }
}
