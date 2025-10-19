using UnityEngine;

public class OrcAttackColider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            DeathCauseManager.MarkKilledByEnemy(2);
            damageable.TakeDamage(damageAmount);
            Debug.Log("Deal damage: " + damageAmount);
        }
    }
}
