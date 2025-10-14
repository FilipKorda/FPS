using UnityEngine;
using UnityEngine.Localization;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Transform transformPosition;
    [SerializeField] private float maxRaycastDistance = 3f;

    [SerializeField] private GameObject platform;

    public LocalizedString localizeStringEvent;

    private IPickupable currentlyPickupableHighlightedObject;
    private IGunPickupable currentlyGunPickupableHighlightedObject;
    private IBridgeController currentlyBridgeController;
    private INpc currentlyNpc;
    private IDoorController currentlyDoorController;
    private ICardHolder currentlyCardHolder;
    private ILinePuzzle currentLinePuzzle;
    private IFuelCan currentFuelCan;
    private IOpenHangar currentOpenHangar;
    private IOxygenHugeContainer currentOxygenHugeContainer;
    private IBarrelForTurretQuest currentBarrelForTurretQuest;
    private IButtonTurretQuest currentButtonTurretQuest;
    private IBackpackPickupable currentBackpackPickupable;


    void Start()
    {
        currentlyBridgeController = platform.GetComponent<IBridgeController>();
    }

    private void Update()
    {
        Vector3 playerPosition = transformPosition.position;
        Vector3 playerDirection = transformPosition.forward;

        if (Physics.Raycast(playerPosition, playerDirection, out RaycastHit hit, maxRaycastDistance))
        {
            currentlyPickupableHighlightedObject?.ResetHighlight();
            currentlyPickupableHighlightedObject?.HideAmmoPackPanel();

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

            currentBackpackPickupable?.ResetHighlight();
            currentBackpackPickupable?.HideHint();
            if (hit.collider.TryGetComponent<IBackpackPickupable>(out var backpackPickupable))
            {
                currentBackpackPickupable = backpackPickupable;
                currentBackpackPickupable.Highlight();
                currentBackpackPickupable.ShowHint();
            }
            else
            {
                currentBackpackPickupable = null;
            }
           


            currentlyBridgeController?.ResetHighlight();
            if (hit.collider.TryGetComponent<IBridgeController>(out var iBridgeController))
            {
                currentlyBridgeController = iBridgeController;
                iBridgeController.Highlight();

                if (currentlyBridgeController.IsPlatformInTheRightPosition())
                {
                    iBridgeController.ResetHighlight();
                }
            }
            else
            {
                currentlyBridgeController = null;
            }

            currentlyNpc?.DeactiveHint();
            if (hit.collider.TryGetComponent<INpc>(out var iNpc))
            {
                if (!DialogueManager.Instance.IsTalking())
                {
                    currentlyNpc = iNpc;
                    iNpc.ActiveHint();
                }
            }
            else
            {
                currentlyNpc = null;
            }

            currentlyDoorController?.DeactiveHint();
            if (hit.collider.TryGetComponent<IDoorController>(out var iDoorController))
            {
                currentlyDoorController = iDoorController;
                iDoorController.ActiveHint();


                if (currentlyDoorController.IsIsOpen())
                {
                    iDoorController.DeactiveHint();
                }
            }
            else
            {
                currentlyDoorController = null;
            }

            currentlyCardHolder?.DeactiveHint();
            if (hit.collider.TryGetComponent<ICardHolder>(out var iCardHolder))
            {
                currentlyCardHolder = iCardHolder;
                iCardHolder.ActiveHint();
            }
            else
            {
                currentlyCardHolder = null;
            }


            currentLinePuzzle?.ResetHighlight();
            if (hit.collider.TryGetComponent<ILinePuzzle>(out var iLinePuzzle))
            {
                currentLinePuzzle = iLinePuzzle;
                if (currentLinePuzzle.IsInLinePuzzle())
                {
                    iLinePuzzle.ResetHighlight();
                }
                else
                {
                    if (currentLinePuzzle.IsInLinePuzzleFinish())
                    {
                        iLinePuzzle.ResetHighlight();
                    }
                    else
                    {
                        iLinePuzzle.Highlight();
                    }

                }
            }
            else
            {
                currentLinePuzzle = null;
            }

            currentFuelCan?.ResetHighlight();
            if (hit.collider.TryGetComponent<IFuelCan>(out var iFuelCan))
            {
                currentFuelCan = iFuelCan;
                if (iFuelCan.IsFuelCan())
                {
                    iFuelCan.ResetHighlight();
                }
                else
                {
                    iFuelCan.Highlight();
                }

            }
            else
            {
                currentFuelCan = null;
            }

            currentOpenHangar?.ResetHighlight();
            if (hit.collider.TryGetComponent<IOpenHangar>(out var iOpenHangar))
            {
                currentOpenHangar = iOpenHangar;

                if (currentOpenHangar.IsOpenGate())
                {
                    iOpenHangar.ResetHighlight();
                }
                else
                {
                    iOpenHangar.Highlight();
                }


            }
            else
            {
                currentOpenHangar = null;
            }

            currentOxygenHugeContainer?.ResetHighlight();
            if (hit.collider.TryGetComponent<IOxygenHugeContainer>(out var iOxygenHugeContainer))
            {
                currentOxygenHugeContainer = iOxygenHugeContainer;

                if (iOxygenHugeContainer.IsRefillingOxygen())
                {
                    iOxygenHugeContainer.ResetHighlight();
                }
                else
                {
                    iOxygenHugeContainer.Highlight();
                }


            }
            else
            {
                currentOxygenHugeContainer = null;
            }

            currentBarrelForTurretQuest?.ResetHighlight();
            if (hit.collider.TryGetComponent<IBarrelForTurretQuest>(out var iBarrelForTurretQuest))
            {
                currentBarrelForTurretQuest = iBarrelForTurretQuest;
                if (iBarrelForTurretQuest.IsBarrelSet())
                {
                    iBarrelForTurretQuest.ResetHighlight();
                }
                else
                {
                    iBarrelForTurretQuest.Highlight();
                }

            }
            else
            {
                currentBarrelForTurretQuest = null;
            }

            currentButtonTurretQuest?.DeactiveHint();
            if (hit.collider.TryGetComponent<IButtonTurretQuest>(out var iButtonTurretQuest))
            {
                currentButtonTurretQuest = iButtonTurretQuest;
                iButtonTurretQuest.ActiveHint();
            }
            else
            {
                currentButtonTurretQuest = null;
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

            if (currentBackpackPickupable != null)
            {
                currentBackpackPickupable.HideHint();
                currentBackpackPickupable.ResetHighlight();
                currentBackpackPickupable = null;
            }
          

            if (currentlyBridgeController != null)
            {
                currentlyBridgeController.ResetHighlight();
                currentlyBridgeController = null;
            }

            if (currentlyNpc != null)
            {
                currentlyNpc.DeactiveHint();
                currentlyNpc = null;
            }

            if (currentlyDoorController != null)
            {
                currentlyDoorController.DeactiveHint();
                currentlyDoorController = null;
            }

            if (currentlyCardHolder != null)
            {
                currentlyCardHolder.DeactiveHint();
                currentlyCardHolder = null;
            }

            if (currentLinePuzzle != null)
            {
                currentLinePuzzle.ResetHighlight();
                currentLinePuzzle = null;
            }

            if (currentFuelCan != null)
            {
                currentFuelCan.ResetHighlight();
                currentFuelCan = null;
            }
            if (currentOpenHangar != null)
            {
                currentOpenHangar.ResetHighlight();
                currentOpenHangar = null;
            }
            if (currentOxygenHugeContainer != null)
            {
                currentOxygenHugeContainer.ResetHighlight();
                currentOxygenHugeContainer = null;
            }
            if (currentBarrelForTurretQuest != null)
            {
                currentBarrelForTurretQuest.ResetHighlight();
                currentBarrelForTurretQuest = null;
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

            if (hit.collider.TryGetComponent<IBackpackPickupable>(out var backpackPickupable))
            {
                backpackPickupable.PickupBackpack();
            }
          

            if (hit.collider.TryGetComponent<IBridgeController>(out var bridgeController))
            {
                bridgeController.ActivateBridge();
            }

            if (!DialogueManager.Instance.IsTalking())
            {
                if (hit.collider.TryGetComponent<INpc>(out var iNpc))
                {
                    iNpc.TalkToNpc();
                }
            }

            if (hit.collider.TryGetComponent<IDoorController>(out var iDoorController))
            {
                if (!iDoorController.IsIsOpen())
                {
                    iDoorController.OpenDoor();
                }
            }

            if (hit.collider.TryGetComponent<ICardHolder>(out var iCardHolder))
            {
                iCardHolder.UseCard();
            }

            if (hit.collider.TryGetComponent<ILinePuzzle>(out var iLinePuzzle))
            {
                if (!iLinePuzzle.IsInLinePuzzleFinish())
                {
                    iLinePuzzle.ActiveLinePuzzle();
                }

            }

            if (hit.collider.TryGetComponent<IFuelCan>(out var iFuelCan))
            {
                iFuelCan.StartLoadFuelCan();
            }

            if (hit.collider.TryGetComponent<IOpenHangar>(out var iOpenHangar))
            {
                if (iOpenHangar.CanOpenGate())
                {
                    iOpenHangar.OpenGateHangar();
                }
            }

            if (hit.collider.TryGetComponent<IOxygenHugeContainer>(out var iOxygenHugeContainer))
            {
                if (!iOxygenHugeContainer.IsRefillingOxygen())
                {
                    iOxygenHugeContainer.StartToRefillOxygen();
                }
            }

            if (hit.collider.TryGetComponent<IBarrelForTurretQuest>(out var iBarrelForTurretQuest))
            {
                iBarrelForTurretQuest.StartInstalBarrel();
            }

            if (hit.collider.TryGetComponent<IButtonTurretQuest>(out var iButtonTurretQuest))
            {
                if (iButtonTurretQuest.IsBarrelSet())
                {
                    iButtonTurretQuest.ActivateTurret();
                }
                else
                {
                    NotificationSystem.Instance.ShowNotification(localizeStringEvent,"Can`t do this, Turret is broken!", 2f);
                }
            }
        }
    }


}
