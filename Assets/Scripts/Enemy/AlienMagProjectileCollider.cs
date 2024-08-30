using UnityEngine;

public class AlienMagProjectileCollider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            damageable.TakeDamage(damageAmount);
            Destroy(gameObject);
            Debug.Log("Deal damage: " + damageAmount);
        }
        else if (!(other.gameObject.layer == LayerMask.NameToLayer("Enemy")))
        {
            Destroy(gameObject);
        }      
    }
}
