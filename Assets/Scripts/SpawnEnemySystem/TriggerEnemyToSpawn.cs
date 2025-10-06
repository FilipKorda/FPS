using UnityEngine;

public class TriggerEnemyToSpawn : MonoBehaviour
{
    [SerializeField] private Collider thisCollider;
    [SerializeField] private EnemySpawnerSystem enemySpawnerSystem;


    private void Start()
    {
        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemySpawnerSystem.StartSpawning();

            thisCollider.enabled = false;
        }
    }
}
