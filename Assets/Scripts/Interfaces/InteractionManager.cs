using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Transform transformPosition;
    public float maxRaycastDistance = 3f;

    private IPickupable currentlyHighlightedObject;
    private void Update()
    {
        Vector3 playerPosition = transformPosition.position;
        Vector3 playerDirection = transformPosition.forward;

        Debug.DrawRay(playerPosition, playerDirection * maxRaycastDistance, Color.red, maxRaycastDistance);

        if (Physics.Raycast(playerPosition, playerDirection, out RaycastHit hit, maxRaycastDistance))
        {
            currentlyHighlightedObject?.ResetHighlight();
            if (hit.collider.TryGetComponent<IPickupable>(out var interactable))
            {
                interactable.Highlight();
                currentlyHighlightedObject = interactable;
            }
            else
            {
                currentlyHighlightedObject = null;
            }
        }
        else
        {
            if (currentlyHighlightedObject != null)
            {
                currentlyHighlightedObject.ResetHighlight();
                currentlyHighlightedObject = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWith();
        }
    }

    private void InteractWith()
    {
        Vector3 playerPosition = transformPosition.position;
        Vector3 playerDirection = transformPosition.forward;

        if (Physics.Raycast(playerPosition, playerDirection, out RaycastHit hit, maxRaycastDistance))
        {
            if (hit.collider.TryGetComponent<IPickupable>(out var interactable))
            {
                interactable.Pickup();
            }
        }
    }


}
