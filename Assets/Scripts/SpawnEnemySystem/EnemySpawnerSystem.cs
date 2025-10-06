using System.Collections;
using UnityEngine;

public class EnemySpawnerSystem : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemys;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float firstSpawnDelay = 5f;
    [SerializeField] private ParticleSystem spawnParticles;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(firstSpawnDelay);
        CameraShake.Instance.AlarmPlayer();
        foreach (var spawnPoint in spawnPoints)
        {
            ParticleSystem particles = Instantiate(spawnParticles, spawnPoint.position, spawnPoint.rotation);
            particles.Play();

            var randomEnemy = enemys[Random.Range(0, enemys.Length)];

            Instantiate(randomEnemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
