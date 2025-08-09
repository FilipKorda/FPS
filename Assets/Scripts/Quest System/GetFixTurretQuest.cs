using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Turret/Fix Turret Condition")]
public class GetFixTurretQuest : Quest
{
    public bool isBarrelSet = false;

    public override bool CheckCompletion()
    {
        return isBarrelSet;
    }
}
