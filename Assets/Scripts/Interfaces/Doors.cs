using DG.Tweening;
using TMPro;
using UnityEngine;

public class Doors : MonoBehaviour, IDoorController
{
    [SerializeField] private GameObject hint_Panel;
    [SerializeField] private TextMeshProUGUI hint_Text;
    private string HintString => "Press [E] to Open Doors";

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

    void Start()
    {
        initialPositionRight = rightDoor.position;
        initialPositionLeft = leftDoor.position;

        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
    }

    public void OpenDoor()
    {
        rightDoor.DOMoveX(rightDoor.position.x + 1f, animationDuration);
        leftDoor.DOMoveX(leftDoor.position.x - 1f, animationDuration);
        isOpen = true;
        DOVirtual.DelayedCall(delayBeforeClosing, CloseDoors);

        if(isOpen)
        {
            door.isOpen = true;
        }
    }

    public void CloseDoors()
    {
        rightDoor.DOMove(initialPositionRight, animationDuration);
        leftDoor.DOMove(initialPositionLeft, animationDuration);
        isOpen = false;

        if(!isOpen)
        {
            door.isOpen = false;
        }
    }

    public void ActiveHint()
    {
        hint_Panel.SetActive(true);
        hint_Text.text = HintString;
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

}
