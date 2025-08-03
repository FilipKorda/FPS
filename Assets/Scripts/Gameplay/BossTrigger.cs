using UnityEngine;
using UnityEngine.Events;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private string bossTag = "EnemyBoss";
    [SerializeField] private UnityEvent eventToPlay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(bossTag))
        {
            eventToPlay?.Invoke();
        }
    }
}
