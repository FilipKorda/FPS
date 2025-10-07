using System.Collections;
using UnityEngine;

public class HurricainActivator : MonoBehaviour
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
            //enemySpawnerSystem.StartSpawning();

            PlayerSingleton.Instance.marsHurricaneController.ActiveHurricaneFog();
            thisCollider.enabled = false;
            StartCoroutine(DeactivateHurricaneAfterTime(PlayerSingleton.Instance.marsHurricaneController.hurricaneDuration));
        }
    }

    private IEnumerator DeactivateHurricaneAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayerSingleton.Instance.marsHurricaneController.DeactiveHurricaneFog();
    }
}
