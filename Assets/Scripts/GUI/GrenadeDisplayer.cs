using TMPro;
using UnityEngine;

public class GrenadeDisplayer : MonoBehaviour
{
    [SerializeField]
    private GrenadeHandler grenadeHandler;
    [SerializeField]
    private TextMeshProUGUI grenadeAmmoText;

    private GrenadeHandler.GrenadeType lastPickedGrenadeType;

    private void Awake()
    {
        grenadeAmmoText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        grenadeHandler.GrenadeTypeChanged += SwitchGranateType;
        UpdateGrenadeCount();
    }

    private void OnDisable()
    {
        grenadeHandler.GrenadeTypeChanged -= SwitchGranateType;
    }


    public void SwitchGranateType(GrenadeHandler.GrenadeType newType)
    {
        lastPickedGrenadeType = newType;
        UpdateGrenadeCount();
    }

    public void UpdateGrenadeCount()
    {
        if (lastPickedGrenadeType == GrenadeHandler.GrenadeType.Regular)
        {
            grenadeAmmoText.text = $"{GrenadeInventory.Instance.currentGranat}";
        }
        else if (lastPickedGrenadeType == GrenadeHandler.GrenadeType.Smoke)
        {
            grenadeAmmoText.text = $"{GrenadeInventory.Instance.currentSmokeGranat}";
        }
    }

}
