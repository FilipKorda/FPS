using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public float detectionRadius = 5f; 


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DetectObjectsAroundPlayer();
        }
    }

    private void DetectObjectsAroundPlayer()
    {
        Vector3 playerPosition = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, detectionRadius);

        foreach (Collider collider in hitColliders)
        {
            
            if (collider.TryGetComponent<IInteractable>(out var objectDetector))
            {
                objectDetector.Interact();
            }
        }
    }
}
