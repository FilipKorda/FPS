using System.Collections;
using UnityEngine;

public class OxygenHugeContainer : MonoBehaviour, IOxygenHugeContainer
{
    public static OxygenHugeContainer Instance;

    private Color[] originalColors;
    private MeshRenderer meshRenderer;

    [SerializeField] private GameObject oxygenCollider;
    [SerializeField] private Transform oxygenRopeStart;
    [SerializeField] private Transform player;
    private LineRenderer lineRenderer;

    public bool isRefillingOxygen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.materials;
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void StartToRefillOxygen()
    {
        PlayerHealth.Instance.IncreaseOxygen();
        oxygenCollider.SetActive(true);
        isRefillingOxygen = true;
    }

    private void Update()
    {
        if (isRefillingOxygen && PlayerHealth.Instance.currentOxygen == 100)
        {
            lineRenderer.enabled = false;

            oxygenCollider.SetActive(false);
            isRefillingOxygen = false;
            PlayerHealth.Instance.isInside = false;
        }

        if (isRefillingOxygen)
        {
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }

            lineRenderer.SetPosition(0, oxygenRopeStart.position);
            lineRenderer.SetPosition(1, PlayerSingleton.Instance.oxygenPipeLink.position);
        }

        if (!PlayerHealth.Instance.isInside)
        {
            oxygenCollider.SetActive(false);
            isRefillingOxygen = false;
            lineRenderer.enabled = false;
        }
    }




    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification("Press [E] to Refil Oxygen");

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

    public bool IsRefillingOxygen()
    {
        return isRefillingOxygen;
    }
}
