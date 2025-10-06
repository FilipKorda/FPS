
using UnityEngine;
[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Backpack/Backpack Condition")]
public class GetBackpackQuest : Quest
{
    public bool isBackpackSet = false;

    public override bool CheckCompletion()
    {
        return isBackpackSet;
    }
}
