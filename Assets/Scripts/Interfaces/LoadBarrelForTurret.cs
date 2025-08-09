using System.Collections;
using UnityEngine;

public class LoadBarrelForTurret : MonoBehaviour, IBarrelForTurretQuest
{
    public GameObject barrelObject;
    [SerializeField] private Animator barrelAnimtor;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem barrelPs;
    [SerializeField] private GetFixTurretQuest getFixTurretQuest;
    private Color[] originalColors;

    public bool isBarrelSet = false;


    private void Start()
    {
        Material[] materials = meshRenderer.materials;
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }


    public void StartInstalBarrel()
    {
        if (MainInventory.Instance.currentBarrels > 0)
        {
            getFixTurretQuest.isBarrelSet = true;
            meshRenderer.enabled = false;
            barrelObject.SetActive(true);
            barrelAnimtor.SetTrigger("Play");
            barrelPs.Play();
            StartCoroutine(PsStop());
            MainInventory.Instance.RemoveBarrel();
            isBarrelSet = true;
        }
        else
        {
            NotificationSystem.Instance.ShowNotification("You dont have Barrel!", 2);
        }
    }

    private IEnumerator PsStop()
    {
        yield return new WaitForSeconds(2.8f);
        barrelPs.Stop();
    }

    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification("Press [E] to Instal Barrel!");

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

    public bool IsBarrelSet()
    {
        return isBarrelSet;
    }
}
