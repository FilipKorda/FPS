using FPS.Enemy;
using UnityEngine;

public class PartHealthAlienMag : MonoBehaviour, IDamageable
{
    public string Name;
    [SerializeField] private int _Health;
    [SerializeField] private int _MaxHealth = 100;
    [SerializeField] private AlienMagEnemyMovement enemyMovement;
    public int CurrentHealth { get => _Health; private set => _Health = value; }
    public int MaxHealth { get => _MaxHealth; private set => _MaxHealth = value; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.ParticleDeathEvent ParticleOnDeath;
    public event IDamageable.DropDeathEvent DropOnDeath;

    [SerializeField] private Transform parent;

    private void OnEnable()
    {
        _Health = MaxHealth;
    }

    public void TakeDamage(int Damage)
    {
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
                if (parent != null)
                {
                    enemyMovement.alienMagAnimator.SetTrigger("DIE");
                    enemyMovement.StopMoving();
                    DropOnDeath?.Invoke(parent.position);
                    ParticleOnDeath?.Invoke(parent.position);
                }
                else
                {
                    enemyMovement.alienMagAnimator.SetTrigger("DIE");
                    enemyMovement.StopMoving();
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
