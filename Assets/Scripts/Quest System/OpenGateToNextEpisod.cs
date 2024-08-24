using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Fuel Can Quest/Open Gate Condition")]
public class OpenGateToNextEpisod : Quest
{
    public override bool CheckCompletion()
    {
        return GameManager.Instance.activeHangarWhenFuelFull.isGateOpen;
    }
}
