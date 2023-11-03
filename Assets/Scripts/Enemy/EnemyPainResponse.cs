using UnityEngine;

namespace FPS.Guns.Demo.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyPainResponse : MonoBehaviour
    {
        [SerializeField]
        private EnemyHealth Health;
        [SerializeField]
        [Range(1, 100)]
        private int MaxDamagePainThreshold = 5;

        public void HandlePain(int Damage)
        {
            if (Health.CurrentHealth != 0)
            {
                // you can do some cool stuff based on the
                // amount of damage taken relative to max health
                // here we're simply setting the additive layer
                // weight based on damage vs max pain threshhold
                Debug.Log("Enemy Die");
            }
        }

        public void HandleDeath()
        {
            Debug.Log("Disaper");
            Destroy(gameObject);
        }
    }
}