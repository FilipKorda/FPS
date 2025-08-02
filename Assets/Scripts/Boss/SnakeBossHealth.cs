using UnityEngine;

namespace FPS.Enemy
{
    public class SnakeBossHealth : MonoBehaviour, IDamageable
    {
        public SnakeBoss boss;
        [SerializeField] private int _Health;
        [SerializeField] private int _MaxHealth = 100;

        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.ParticleDeathEvent ParticleOnDeath;
        public event IDamageable.DropDeathEvent DropOnDeath;


        public int CurrentHealth
        {
            get => _Health;
            private set => _Health = value;
        }

        public int MaxHealth
        {
            get => _MaxHealth;
            private set => _MaxHealth = value;
        }

        private void OnEnable()
        {
            _Health = MaxHealth;
        }

        public void TakeDamage(int Damage)
        {
            int damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);

            CurrentHealth -= damageTaken;

            if (damageTaken != 0)
            {
                OnTakeDamage?.Invoke(damageTaken);
            }

            if (CurrentHealth == 0 && damageTaken != 0)
            {
                DropOnDeath?.Invoke(transform.position);
                ParticleOnDeath?.Invoke(transform.position);
            }
        }

        void Die()
        {
            Debug.Log("SnakeBoss zginął!");
            // Tutaj możesz dodać animację śmierci / zniszczenie
            Destroy(gameObject);
        }
    }
}