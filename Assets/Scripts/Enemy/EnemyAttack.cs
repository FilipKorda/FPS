using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Collider standAttackCollider;
    [SerializeField] private Collider crawlAttackCollider;
 
    private void Start()
    {
        standAttackCollider.enabled = false;
        crawlAttackCollider.enabled = false;
    }

    public void SetStandAttackCollider()
    {
        standAttackCollider.enabled = true;
        crawlAttackCollider.enabled = false;
    }

    public void SetCrawlAttackCollider()
    {
        crawlAttackCollider.enabled = true;
        standAttackCollider.enabled = false;
    }

    public void DisableAttackColliders()
    {
        standAttackCollider.enabled = false;
        crawlAttackCollider.enabled = false;
    }
}
