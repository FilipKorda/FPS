using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeSelector : MonoBehaviour
{
    [SerializeField]
    private GrenadeThrower grenadeThrower;
    [SerializeField]
    private Image grenadeIcon;      
    [SerializeField]
    private TextMeshProUGUI grenadeNameText;

    private void OnEnable()
    {
        //grenadeThrower.OnGrenadeSelectionChanged += UpdateGrenadeSelector;
    }

    private void OnDisable()
    {
      //  grenadeThrower.OnGrenadeSelectionChanged -= UpdateGrenadeSelector;
    }

    private void UpdateGrenadeSelector(int activeListIndex)
    {
        if (grenadeThrower.activeListIndex == 0)
        {
            Sprite gunIconOne = grenadeThrower.grenade.GrenadeIcon;
            grenadeIcon.sprite = gunIconOne;
            grenadeNameText.text = grenadeThrower.grenade.name;
        }
        else if (grenadeThrower.activeListIndex == 1)
        {
            Sprite gunIconOne = grenadeThrower.smokeGrenade.GrenadeIcon;
            grenadeIcon.sprite = gunIconOne;
            grenadeNameText.text = grenadeThrower.smokeGrenade.name;
        }       
    }
}
