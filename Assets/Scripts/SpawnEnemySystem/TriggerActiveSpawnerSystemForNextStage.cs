using UnityEngine;
using UnityEngine.Events;

public class TriggerActiveSpawnerSystemForNextStage : MonoBehaviour
{
    [SerializeField] private Collider thisCollider;

    [SerializeField] private UnityEvent unityEvent;


    private void Start()
    {
        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            unityEvent.Invoke();

            thisCollider.enabled = false;
        }
    }
}
