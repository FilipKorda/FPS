using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class LoadBarrelForTurret : MonoBehaviour, IBarrelForTurretQuest
{
    public GameObject barrelObject;
    [SerializeField] private Animator barrelAnimtor;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem barrelPs;
    [SerializeField] private GetFixTurretQuest getFixTurretQuest;
    private Color[] originalColors;

    public bool isBarrelSet = false;

    public LocalizedString localizeStringEvent;
    public LocalizedString localizeStringEventPress;

    [SerializeField] private AudioClip repairSound;

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
            AudioManager.Instance.PlayClip(repairSound, transform.position, 0.5f, false, 1, 500, 1, false, null);
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
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You dont have Barrel!", 2);
        }
    }

    private IEnumerator PsStop()
    {
        yield return new WaitForSeconds(2.8f);
        barrelPs.Stop();
    }

    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification(localizeStringEventPress, "Press [E] to Instal Barrel!");

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
