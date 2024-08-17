using UnityEngine;
[CreateAssetMenu(fileName = "New Card Quest Condition", menuName = "Quests/Card Quest/Blue Card Condition")]
public class GetBlueCardQuest : Quest
{
    public override bool CheckCompletion()
    {
        return MainInventory.Instance.blueCard == 1;
    }
}
