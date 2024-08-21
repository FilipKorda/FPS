using System.Collections;
using UnityEngine;

public class NextEpisod : AvalancheMeteorites
{
    [SerializeField] private GameObject enemySpawnSystemToDeactive;
    [SerializeField] private GameObject enemySpawnSystemToActive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerColideWithCollider)
            {
                shouldMove = true;
                playerColideWithCollider = true;
                CameraShake.Instance.AlarmPlayer();
            }

            enemySpawnSystemToDeactive.SetActive(false);
            enemySpawnSystemToActive.SetActive(true);
            StartCoroutine(DeactiveCollider());
        }
    }

    IEnumerator DeactiveCollider()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
