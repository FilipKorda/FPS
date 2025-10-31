using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IEnemyDamagable>(out var damageable))
        {
            DeathCauseManager.MarkKilledByEnemy(0);
            // przeka¿ pozycjê atakuj¹cego (ten collider nale¿y do wroga)
            damageable.TakeDamage(damageAmount, transform.position);
            Debug.Log("Deal damage: " + damageAmount);
        }
    }
}
