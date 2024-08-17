using UnityEngine;
[CreateAssetMenu(fileName = "New Card Quest Condition", menuName = "Quests/Card Quest/Green Card Condition")]
public class GetGreenCardQuest : Quest
{
    public override bool CheckCompletion()
    {
        return MainInventory.Instance.greenCard == 1;
    }
}
