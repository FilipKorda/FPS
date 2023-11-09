using FPS.Guns.Demo;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Transform transformPosition;
    [SerializeField] private float maxRaycastDistance = 3f;

    private IPickupable currentlyHighlightedObject;

    private void Update()
    {
        Vector3 playerPosition = transformPosition.position;
        Vector3 playerDirection = transformPosition.forward;

        if (Physics.Raycast(playerPosition, playerDirection, out RaycastHit hit, maxRaycastDistance))
        {
            currentlyHighlightedObject?.ResetHighlight();
            currentlyHighlightedObject?.HideAmmoPackPanel();
            if (hit.collider.TryGetComponent<IPickupable>(out var interactable))
            {
                currentlyHighlightedObject = interactable;
                interactable.Highlight();
                interactable.ShowAmmoPackPanel();
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
                currentlyHighlightedObject.HideAmmoPackPanel();
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
