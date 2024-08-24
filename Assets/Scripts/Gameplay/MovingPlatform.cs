using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterController.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterController.transform.SetParent(null);
        }
    }
}
