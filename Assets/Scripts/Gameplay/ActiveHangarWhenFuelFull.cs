using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;

public class ActiveHangarWhenFuelFull : MonoBehaviour, IOpenHangar
{
    [SerializeField] private GameObject gate;
    [SerializeField] private CardReader cardReader;
    [SerializeField] private LoadFuelCan loadFuelCan;
    [SerializeField] private LoadFuelCan loadFuelCan1;
    [SerializeField] private LoadFuelCan loadFuelCan2;
    [SerializeField] private LoadFuelCan loadFuelCan3;
    private MeshRenderer meshRenderer;
    private Color[] originalColors;
    public bool isGateOpen;
    [SerializeField] private float timeUntilGateOpen = 1f;
    [SerializeField] private float heightToGateOpen = 2.75f;

    [SerializeField] private GameObject buttonObj;

    [SerializeField] private float buttonPressDepth = 0.02f;
    [SerializeField] private float buttonPressTime = 0.1f;

    public LocalizedString localizeStringEventOpenGate;
    public LocalizedString localizeStringEventFindFuelTank;

    public GameObject notificationObject;
    public AudioClip openGateClip;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.materials;
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    public void OpenGateHangar()
    {
        notificationObject.SetActive(false);

        PlayButtonLocalYAnimation();

        gate.transform.DOMoveY(gate.transform.position.y + heightToGateOpen, timeUntilGateOpen)
            .SetEase(Ease.OutQuad);
        isGateOpen = true;
    }

    public void Highlight()
    {
        if (CanOpenGate())
        {
            NotificationSystem.Instance.ShowInfiniteNotification(localizeStringEventOpenGate, "Press [E] to OpenGate!");
        }
        else
        {
            NotificationSystem.Instance.ShowInfiniteNotification(localizeStringEventFindFuelTank, "Find Fuel Tank and red card to Active This Button");
        }

        foreach (Material mat in meshRenderer.materials)
        {
            mat.color = Color.yellow;
        }
    }

    public void ResetHighlight()
    {
        NotificationSystem.Instance.HideInfiniteNotification();

        Material[] materials = meshRenderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }

    public bool CanOpenGate()
    {
        return cardReader.isCardRead & loadFuelCan.isFuelFull && loadFuelCan1.isFuelFull & loadFuelCan2.isFuelFull && loadFuelCan3.isFuelFull;
    }

    public bool IsOpenGate()
    {
        return isGateOpen;
    }

    [ContextMenu("!!!!!!!!!!!!!!")]
    public void PlayButtonLocalYAnimation()
    {
        if (buttonObj == null) return;
        AudioManager.Instance.PlayClip(openGateClip, transform.position, 0.6f, false, 1, 500, 1, false, null);

        var t = buttonObj.transform;

        t.DOKill(complete: true);

        var startPos = t.position;         
        var pressDir = -t.up;              

        Sequence seq = DOTween.Sequence();
        seq.Append(t.DOMove(startPos + pressDir * buttonPressDepth, buttonPressTime).SetEase(Ease.OutQuad));
        seq.Append(t.DOMove(startPos,                          buttonPressTime).SetEase(Ease.InQuad));
    }
}
