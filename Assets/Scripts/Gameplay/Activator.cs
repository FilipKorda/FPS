using UnityEngine;
using UnityEngine.Events;

public class Activator : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            unityEvent.Invoke();
        }
    }
}
