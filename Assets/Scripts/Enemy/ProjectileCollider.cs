using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10;
    [SerializeField] private ParticleSystem collisonEffectPS;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            damageable.TakeDamage(damageAmount);
            Destroy(gameObject);
            PlayCollisionEffetct();
            Debug.Log("Deal damage: " + damageAmount);
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
