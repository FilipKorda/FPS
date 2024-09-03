using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Collider standAttackCollider;
    [SerializeField] private Collider crawlAttackCollider;
    [SerializeField] private Collider swordAttackCollider;

    private void Start()
    {
        standAttackCollider.enabled = false;
        crawlAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

    public void SetStandAttackCollider()
    {
        standAttackCollider.enabled = true;
        crawlAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

    public void SetCrawlAttackCollider()
    {
        crawlAttackCollider.enabled = true;
        standAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

    public void SetSwordAttackCollider()
    {
        crawlAttackCollider.enabled = false;
        standAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = true;
    }


    public void DisableAttackColliders()
    {
        standAttackCollider.enabled = false;
        crawlAttackCollider.enabled = false;
        if (swordAttackCollider != null)
            swordAttackCollider.enabled = false;
    }

}
