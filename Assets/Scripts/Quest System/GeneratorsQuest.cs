using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Condition", menuName = "Quests/Fuel Can Quest/Generators Condition")]
public class GeneratorsQuest : Quest
{
    public override bool CheckCompletion()
    {
        // Zwraca true, jeœli wszystkie elementy w loadFuelCans maj¹ isFuelFull == true
        return GameManager.Instance.loadFuelCans.All(loadFuelCan => loadFuelCan.isFuelFull);
    }
}
