using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyPainResponse : MonoBehaviour
    {
        [SerializeField] private GameObject wholeEnemy;
        [SerializeField]
        private PartHealth Health;
        [SerializeField]
        [Range(1, 100)]
        private readonly int MaxDamagePainThreshold = 5;

        public void HandlePain(int Damage)
        {
            if (Health.CurrentHealth != 0)
            {
                Debug.Log("Enemy Get Hit: " + gameObject.name + "" + Damage);
            }
        }

        public void HandleDeath()
        {
            Debug.Log("Destroy");
            Destroy(gameObject);

        }

        public void HandleAllPartDeath()
        {
            if (wholeEnemy != null)
            {
                Destroy(wholeEnemy);
            }
        }

    }
}