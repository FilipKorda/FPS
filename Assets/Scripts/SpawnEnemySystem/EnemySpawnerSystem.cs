using FPS.Guns.Demo;
using System.Collections;
using UnityEngine;

public class EnemySpawnerSystem : MonoBehaviour
{
    [SerializeField] private EnemyData enemy;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float firstSpawnDelay = 5f;
    [SerializeField] private ParticleSystem spawnParticles;

    //Camera shake to alarm player
    [SerializeField] private float cameraShakeDuration = 0.75f;
    [SerializeField] private float cameraShakeMagnitude = 0.1f;

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
        AlarmPlayer();
        foreach (var spawnPoint in spawnPoints)
        {
            ParticleSystem particles = Instantiate(spawnParticles, spawnPoint.position, spawnPoint.rotation);
            particles.Play();
            Instantiate(enemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void AlarmPlayer()
    {
        StartCoroutine(Shake(cameraShakeDuration, cameraShakeMagnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = PlayerGunSelector.Instance.Camera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            PlayerGunSelector.Instance.Camera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
