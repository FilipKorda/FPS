using UnityEngine;

public class LoadFuelCan : MonoBehaviour, IFuelCan
{
    public GameObject fuelCanAnimation;
    [SerializeField] private Animator fuelCanAnimtor;
    [SerializeField] private Animator cableAnimation;
    public GameObject fuelCanEndHolder;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem fuelPs;
    private Color[] originalColors;

    public bool isFuelFull = false;
    private bool isFuelCan = false;

    private void Start()
    {
        Material[] materials = meshRenderer.materials;
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    public void StartLoadFuelCan()
    {
        if (MainInventory.Instance.currentfuelCans > 0)
        {
            meshRenderer.enabled = false;
            fuelCanAnimation.SetActive(true);
            fuelCanAnimtor.SetTrigger("Play");
            cableAnimation.SetTrigger("Play");
            fuelPs.Play();
            MainInventory.Instance.RemoveFuelCan();
            isFuelFull = true;
            isFuelCan = true;
        }
        else
        {
            NotificationSystem.Instance.ShowNotification("You dont have Fuel Can!", 2);
        }

    }

    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification("Press [E] to Load Fuel!");

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

    public bool IsFuelCan()
    {
        return isFuelCan;
    }

}
