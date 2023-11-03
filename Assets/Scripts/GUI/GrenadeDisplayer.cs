using TMPro;
using UnityEngine;

public class GrenadeDisplayer : MonoBehaviour
{
    [SerializeField]
    private GrenadeHandler grenadeHandler;
    [SerializeField]
    private TextMeshProUGUI grenadeAmmoText;

    private void Awake()
    {
        grenadeAmmoText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        grenadeHandler.GrenadeTypeChanged += SwitchGranateType;
    }

    private void OnDisable()
    {
        grenadeHandler.GrenadeTypeChanged -= SwitchGranateType;
    }

    public void SwitchGranateType(GrenadeHandler.GrenadeType newType)
    {
        if (newType == GrenadeHandler.GrenadeType.Regular)
        {
            grenadeAmmoText.text = $"{GrenadeInventory.Instance.currentGranat}";
        }
        else if (newType == GrenadeHandler.GrenadeType.Smoke)
        {
            grenadeAmmoText.text = $"{GrenadeInventory.Instance.currentSmokeGranat}";
        }
    }

}
