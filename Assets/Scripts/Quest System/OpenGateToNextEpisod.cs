using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Fuel Can Quest/Open Gate Condition")]
public class OpenGateToNextEpisod : Quest
{
    public override bool CheckCompletion()
    {
        var gm = GameManager.Instance;
        if (gm == null || gm.activeHangarWhenFuelFull == null)
            return false; 

        return gm.activeHangarWhenFuelFull.isGateOpen;
    }
}
