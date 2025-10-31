using UnityEngine;

public class RobotAttackColider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 15;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            DeathCauseManager.MarkKilledByEnemy(3);
            damageable.TakeDamage(damageAmount, transform.position);
            Debug.Log("Deal damage: " + damageAmount);
        }
    }
}
