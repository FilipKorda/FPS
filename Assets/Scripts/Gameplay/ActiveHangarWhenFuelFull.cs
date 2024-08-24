using DG.Tweening;
using UnityEngine;

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
        gate.transform.DOMoveY(gate.transform.position.y + heightToGateOpen, timeUntilGateOpen).SetEase(Ease.OutQuad);
        isGateOpen = true;
    }

    public void Highlight()
    {
        if (CanOpenGate())
        {
            NotificationSystem.Instance.ShowInfiniteNotification("Press [E] to OpenGare!");
        }
        else
        {
            NotificationSystem.Instance.ShowInfiniteNotification("Find Fuel Tank and red card to Active This Button");
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
}
