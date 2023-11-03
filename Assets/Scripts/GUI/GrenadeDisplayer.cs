using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrenadeDisplayer : MonoBehaviour
{
    [SerializeField]
    private GrenadeThrower grenadeThrower;
    [SerializeField]
    private TextMeshProUGUI greandeAmmoText;

    private void Awake()
    {
        greandeAmmoText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
       // grenadeThrower.GrenadeChangedOnGUI += UpdateGrenadeCount;
    }

    private void OnDisable()
    {
       // grenadeThrower.GrenadeChangedOnGUI -= UpdateGrenadeCount;
    }

    private void UpdateGrenadeCount(List<GameObject> activeGrenadeList)
    {
        int currentGrenadeCount = activeGrenadeList.Count;
        greandeAmmoText.text = $"{currentGrenadeCount}";
    }

}
