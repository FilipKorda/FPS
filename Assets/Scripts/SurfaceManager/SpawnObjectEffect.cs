using UnityEngine;

namespace FPS.ImpactSystem.Effects
{
    [CreateAssetMenu(menuName = "Impact System/Spawn Object Effect", fileName = "SpawnObjectEffect")]
    public class SpawnObjectEffect : ScriptableObject
    {
        public GameObject Prefab;
    }
}