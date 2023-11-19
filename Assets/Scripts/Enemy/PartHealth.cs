using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class PartHealth : MonoBehaviour, IDamageable
    {
        public string Name;
        [SerializeField]
        private int _Health;
        [SerializeField]
        private int _MaxHealth = 100;
        public int CurrentHealth { get => _Health; private set => _Health = value; }
        public int MaxHealth { get => _MaxHealth; private set => _MaxHealth = value; }

        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.ParticleDeathEvent ParticleOnDeath;
        public event IDamageable.DropDeathEvent DropOnDeath;

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
                if (Name == "Head" || Name == "Body")
                {
                    DropOnDeath?.Invoke(transform.position);
                    ParticleOnDeath?.Invoke(transform.position);
                }
                else
                {
                    ParticleOnDeath?.Invoke(transform.position);
                }

            }
        }
    }
}