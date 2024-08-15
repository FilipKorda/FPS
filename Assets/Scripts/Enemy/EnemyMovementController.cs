using UnityEngine;

namespace FPS.Enemy
{
    public class EnemyMovementController : MonoBehaviour
    {
        public static EnemyMovementController Instance { get; private set; }
        public EnemyMovement Movement;
        private void Awake()
        {
            Instance = this;
        }
    }
}
