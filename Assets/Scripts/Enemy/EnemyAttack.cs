using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Collider standAttackCollider;
    [SerializeField] private Collider crawlAttackCollider;
    [SerializeField] private Collider swordAttackCollider;

    private void Start()
    {
        if (standAttackCollider != null)
            standAttackCollider.enabled = false;
        if (crawlAttackCollider != null)
            crawlAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

    public void SetStandAttackCollider()
    {
        if (standAttackCollider != null)
            standAttackCollider.enabled = true;
        if (crawlAttackCollider != null)
            crawlAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

    public void SetCrawlAttackCollider()
    {
        if (crawlAttackCollider != null)
            crawlAttackCollider.enabled = true;
        if (standAttackCollider != null)
            standAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

    public void SetSwordAttackCollider()
    {
        if (crawlAttackCollider != null)
            crawlAttackCollider.enabled = false;
        if (standAttackCollider != null)
            standAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = true;
    }


    public void DisableAttackColliders()
    {
        if (standAttackCollider != null)
            standAttackCollider.enabled = false;
        if (crawlAttackCollider != null)
            crawlAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

}
