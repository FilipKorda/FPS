using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class Doors : MonoBehaviour, IDoorController
{
    [SerializeField] private GameObject hint_Panel;
    [SerializeField] private TextMeshProUGUI hint_Text;

    [SerializeField] private Transform rightDoor;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float delayBeforeClosing = 2f;
    public bool isOpen;
    private Vector3 initialPositionRight;
    private Vector3 initialPositionLeft;
    private Color originalColor;
    private Renderer originalColorRenderer;
    [SerializeField] private Doors door;

    [Header("Button Press")]
    [SerializeField] private Transform buttonTransform;            
    [SerializeField] private float buttonPressDepth = 0.02f;        
    [SerializeField] private float buttonPressDuration = 0.06f;      
    [SerializeField] private float buttonReleaseDuration = 0.10f;    
    [SerializeField] private Ease buttonPressEase = Ease.OutSine;
    [SerializeField] private Ease buttonReleaseEase = Ease.InSine;
    private Vector3 initialLocalButtonPos;

    public LocalizedString localizeStringEvent;

    [SerializeField] private AudioClip openCloseDoor;

    void Start()
    {
        initialPositionRight = rightDoor.position;
        initialPositionLeft = leftDoor.position;

        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;

        if (buttonTransform != null)
        {
            initialLocalButtonPos = buttonTransform.localPosition;
        }
    }

    public void OpenDoor()
    {

        AudioManager.Instance.PlayClip(openCloseDoor, transform.position, 0.25f, true, 1, 500, 1, false, null);
        rightDoor.DOMoveX(rightDoor.position.x + 1f, animationDuration);
        leftDoor.DOMoveX(leftDoor.position.x - 1f, animationDuration);
        isOpen = true;
        DOVirtual.DelayedCall(delayBeforeClosing, CloseDoors);

        AnimateButtonPress();

        if (isOpen)
        {
            door.isOpen = true;
        }
    }

    public void CloseDoors()
    {
        AudioManager.Instance.PlayClip(openCloseDoor, transform.position, 0.5f, false, 1, 500, 1, false, null);
        rightDoor.DOMove(initialPositionRight, animationDuration);
        leftDoor.DOMove(initialPositionLeft, animationDuration);
        isOpen = false;

        if (!isOpen)
        {
            door.isOpen = false;
        }
    }

    public void ActiveHint()
    {
        hint_Panel.SetActive(true);

        hint_Text.text = localizeStringEvent != null
         ? localizeStringEvent.GetLocalizedString()
         : string.Empty;

        originalColorRenderer.material.color = Color.yellow;
    }

    public void DeactiveHint()
    {
        hint_Panel.SetActive(false);
        hint_Text.text = "";
        originalColorRenderer.material.color = originalColor;
    }

    public bool IsIsOpen()
    {
        return isOpen;
    }

    private void AnimateButtonPress()
    {
        if (buttonTransform == null) return;

        buttonTransform.DOKill();

        float targetZ = initialLocalButtonPos.z - buttonPressDepth; 
        Sequence seq = DOTween.Sequence();
        seq.Append(buttonTransform.DOLocalMoveZ(targetZ, buttonPressDuration).SetEase(buttonPressEase));
        seq.Append(buttonTransform.DOLocalMoveZ(initialLocalButtonPos.z, buttonReleaseDuration).SetEase(buttonReleaseEase));
    }
}
