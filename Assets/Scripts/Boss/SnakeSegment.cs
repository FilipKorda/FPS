using UnityEngine;

namespace FPS.Enemy
{
    public class SnakeSegment : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _Health;
        [SerializeField] private int _MaxHealth = 100;

        public int CurrentHealth { get => _Health; private set => _Health = value; }
        public int MaxHealth { get => _MaxHealth; private set => _MaxHealth = value; }

        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.ParticleDeathEvent ParticleOnDeath;
        public event IDamageable.DropDeathEvent DropOnDeath;

        public SnakeBossHealth bossHealth;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
        }

        public void ApplyDamage(int damage)
        {
            int damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);
            CurrentHealth -= damageTaken;

            if (damageTaken > 0)
            {
                OnTakeDamage?.Invoke(damageTaken);
            }

            if (CurrentHealth <= 0 && damageTaken > 0)
            {
                DropOnDeath?.Invoke(transform.position);
                ParticleOnDeath?.Invoke(transform.position);
            }
        }
    }
}
