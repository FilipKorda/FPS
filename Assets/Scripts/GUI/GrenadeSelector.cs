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
            Sprite grenadeIconSprite = grenadeHandler.granatPrefab.GrenadeIcon;
            grenadeIcon.sprite = grenadeIconSprite;
            grenadeNameText.text = grenadeHandler.granatPrefab.name;
        }
        else if (newType == GrenadeHandler.GrenadeType.Smoke)
        {
            Sprite smokeGrenadeIconSprite = grenadeHandler.smokeGranatPrefab.GrenadeIcon;
            grenadeIcon.sprite = smokeGrenadeIconSprite;
            grenadeNameText.text = grenadeHandler.smokeGranatPrefab.name;         
        }
    }


}
