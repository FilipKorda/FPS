using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10;
    [SerializeField] private ParticleSystem collisonEffectPS;

    public bool isRobotProjectile = false;


    [SerializeField] private AudioClip projectileHitSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            damageable.TakeDamage(damageAmount, transform.position);
            Destroy(gameObject);
            PlayCollisionEffetct();

            AudioManager.Instance.PlayClip(projectileHitSound, transform.position, 0.01f, true, 1, 500, 1, false, transform);

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
