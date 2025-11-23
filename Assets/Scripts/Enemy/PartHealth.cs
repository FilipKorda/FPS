using UnityEngine;
using UnityEngine.UIElements;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class PartHealth : MonoBehaviour, IDamageable
    {
        public string Name;
        [SerializeField] private int _Health;
        [SerializeField] private int _MaxHealth = 100;
        [SerializeField] private EnemyMovement enemyMovement;
        public int CurrentHealth { get => _Health; private set => _Health = value; }
        public int MaxHealth { get => _MaxHealth; private set => _MaxHealth = value; }

        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.ParticleDeathEvent ParticleOnDeath;
        public event IDamageable.DropDeathEvent DropOnDeath;

        [SerializeField] private Transform parent;

        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip deathSound;

        private void OnEnable()
        {
            _Health = MaxHealth;
        }

        public void TakeDamage(int Damage)
        {
            AudioManager.Instance.PlayClip(hitSound, transform.position, 0.01f, true, 1, 500, 1, false, transform);

            if (enemyMovement != null)
            {
                enemyMovement.StartFollowPlayerAFterHit();
            }

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
                    AudioManager.Instance.PlayClip(deathSound, transform.position, 0.1f, true, 1, 500, 1, false, transform);

                    if (parent != null)
                    {
                        DropOnDeath?.Invoke(parent.position);
                        ParticleOnDeath?.Invoke(parent.position);
                    }
                    else
                    {
                        DropOnDeath?.Invoke(transform.position);
                        ParticleOnDeath?.Invoke(transform.position);
                    }
                }
                else
                {
                    if (parent != null)
                    {
                        ParticleOnDeath?.Invoke(parent.position);
                    }
                    else
                    {
                        ParticleOnDeath?.Invoke(transform.position);
                    }
                }

            }
        }
    }
}