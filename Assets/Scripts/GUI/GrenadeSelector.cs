using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeSelector : MonoBehaviour
{
    [SerializeField]
    private GrenadeHandler grenadeHandler;
    [SerializeField]
    private Image grenadeIcon;      
    [SerializeField]
    private TextMeshProUGUI grenadeNameText;

    private void OnEnable()
    {
        grenadeHandler.GrenadeTypeChanged += UpdateGrenadeSelector;
    }

    private void OnDisable()
    {
        grenadeHandler.GrenadeTypeChanged -= UpdateGrenadeSelector;
    }

    private void UpdateGrenadeSelector(GrenadeHandler.GrenadeType newType)
    {
        if (newType == GrenadeHandler.GrenadeType.Regular)
        {
            var so = grenadeHandler.granatPrefab;
            grenadeIcon.sprite = so.GrenadeIcon;
            grenadeNameText.text = so.GetLocalizedName();
        }
        else if (newType == GrenadeHandler.GrenadeType.Smoke)
        {
            var so = grenadeHandler.smokeGranatPrefab;
            grenadeIcon.sprite = so.GrenadeIcon;
            grenadeNameText.text = so.GetLocalizedName();
        }
    }
}
