using UnityEngine;

namespace FPS.Enemy
{
    public class EnemyMovementController : MonoBehaviour
    {
        public static EnemyMovementController Instance { get; private set; }

        [SerializeField] public EnemyMovement Movement;

        private void Awake()
        {
            Instance = this;
        }
    }
}
