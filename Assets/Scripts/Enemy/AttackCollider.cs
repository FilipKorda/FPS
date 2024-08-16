
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            damageable.TakeDamage(damageAmount);
            Debug.Log("Deal damage: " + damageAmount);
        }
    }
}
