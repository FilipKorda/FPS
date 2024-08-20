using FPS.Guns.Demo;
using System.Collections;
using UnityEngine;

public class EnemySpawnerSystem : MonoBehaviour
{
    [SerializeField] private EnemyData enemy;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float firstSpawnDelay = 5f;
    [SerializeField] private ParticleSystem spawnParticles;
    
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
        CameraShake.Instance.AlarmPlayer();
        foreach (var spawnPoint in spawnPoints)
        {
            ParticleSystem particles = Instantiate(spawnParticles, spawnPoint.position, spawnPoint.rotation);
            particles.Play();
            Instantiate(enemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

}
