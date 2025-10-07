using UnityEngine;

public class BossDealDamage : MonoBehaviour
{
    [SerializeField] private Collider thisCollider;
    [SerializeField] private float damageAmount = 15f;

    private void Start()
    {
        if (thisCollider == null)
            thisCollider = GetComponent<Collider>();

        if (thisCollider == null)
        {
            Debug.LogError("BossDealDamage: Brak Collidera na obiekcie bossa.");
            return;
        }

        thisCollider.enabled = true;
        thisCollider.isTrigger = true;

        var rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.isDead && playerHealth.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damageAmount);
            return;
        }

        var damagable = other.GetComponentInParent<IEnemyDamagable>();
        if (damagable != null && other.CompareTag("Player"))
        {
            damagable.TakeDamage(damageAmount);
        }
    }
}
