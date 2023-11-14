using FPS.Guns.Demo;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Transform transformPosition;
    [SerializeField] private float maxRaycastDistance = 3f;

    private IPickupable currentlyPickupableHighlightedObject;
    private IGunPickupable currentlyGunPickupableHighlightedObject;
    private IBridgeController currentlyBridgeController;

    private void Update()
    {
        Vector3 playerPosition = transformPosition.position;
        Vector3 playerDirection = transformPosition.forward;

        if (Physics.Raycast(playerPosition, playerDirection, out RaycastHit hit, maxRaycastDistance))
        {
            currentlyPickupableHighlightedObject?.ResetHighlight();
            currentlyPickupableHighlightedObject?.HideAmmoPackPanel();
          //  currentlyBridgeController?.ResetHighlight();

            if (hit.collider.TryGetComponent<IPickupable>(out var interactable))
            {
                currentlyPickupableHighlightedObject = interactable;
                interactable.Highlight();
                interactable.ShowAmmoPackPanel();
            }
            else
            {
                currentlyPickupableHighlightedObject = null;
            }


            currentlyGunPickupableHighlightedObject?.HideNotification();
            if (hit.collider.TryGetComponent<IGunPickupable>(out var gunPickupable))
            {
                currentlyGunPickupableHighlightedObject = gunPickupable;
                gunPickupable.ShowNotification();
            }
            else
            {
                currentlyGunPickupableHighlightedObject = null;
            }


            currentlyBridgeController?.ResetHighlight();
            if (hit.collider.TryGetComponent<IBridgeController>(out var iBridgeController))
            {
                currentlyBridgeController = iBridgeController;
                iBridgeController.Highlight();

            }
            else
            {
                currentlyBridgeController = null;
            }

        }
        else
        {
            if (currentlyPickupableHighlightedObject != null)
            {
                currentlyPickupableHighlightedObject.HideAmmoPackPanel();
                currentlyPickupableHighlightedObject.ResetHighlight();
                currentlyPickupableHighlightedObject = null;
            }

            if (currentlyGunPickupableHighlightedObject != null)
            {
                currentlyGunPickupableHighlightedObject.HideNotification();
                currentlyGunPickupableHighlightedObject = null;
            }

            if (currentlyBridgeController != null)
            {
                currentlyBridgeController.ResetHighlight();
                currentlyBridgeController = null;
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
                interactable.PickupAmmo();
            }

            if (hit.collider.TryGetComponent<IGunPickupable>(out var gunPickupable))
            {
                gunPickupable.PickupGun();
            }

            if (hit.collider.TryGetComponent<IBridgeController>(out var bridgeController))
            {
                bridgeController.ActivateBridge();
            }
        }
    }


}
