using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10;
    [SerializeField] private ParticleSystem collisonEffectPS;

    public bool isRobotProjectile = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            damageable.TakeDamage(damageAmount);
            Destroy(gameObject);
            PlayCollisionEffetct();
            if (isRobotProjectile)
            {
                DeathCauseManager.MarkKilledByEnemy(3);
            }
            else
            {
                DeathCauseManager.MarkKilledByEnemy(1);
            }
            Debug.Log("Deal damage: " + damageAmount);
            StatisticsCollector.AddDamage(damageAmount);
        }
        else if (!(other.gameObject.layer == LayerMask.NameToLayer("Enemy")))
        {
            PlayCollisionEffetct();
            Destroy(gameObject);
        }

    }

    private void PlayCollisionEffetct()
    {
        Instantiate(collisonEffectPS.gameObject, transform.position, transform.rotation);
    }
}
