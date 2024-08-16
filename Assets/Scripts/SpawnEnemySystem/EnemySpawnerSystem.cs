using System.Collections;
using UnityEngine;

public class EnemySpawnerSystem : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float firstSpawnDelay = 5f; // OpóŸnienie przed pierwszym spawnowaniem


    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
    }


    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(firstSpawnDelay);

        while (true)
        {
            foreach (var enemy in enemies)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                yield return new WaitForSeconds(enemy.spawnRate);
            }
        }
    }

}
