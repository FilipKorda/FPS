using UnityEngine;

[CreateAssetMenu(fileName = "New Card Quest Condition", menuName = "Quests/Card Quest/Red Card Condition")]
public class GetRedCardQuest : Quest
{
    public override bool CheckCompletion()
    {
        return MainInventory.Instance.redCard == 1;
    }
}
