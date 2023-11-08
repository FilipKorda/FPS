using DG.Tweening;
using FPS.Guns.Demo;
using TMPro;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IPickupable
{
    [SerializeField] private PlayerGunSelector playerGunSelector;

    private Color originalColor;
    private new Renderer renderer;

    [SerializeField] private GameObject AmmoPackPanel;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private string headerString = "Ammo Pack";
    [SerializeField] private TextMeshProUGUI mainFirstText;
    [SerializeField] private string mainFirstString = "Available amount of Ammo";
    [SerializeField] private TextMeshProUGUI totalAmmoText;
    [SerializeField] private int totalAmmo = 100;

    [SerializeField] private Vector3 hide = new(1100f, 0f, 0f);
    [SerializeField] private Vector3 show = new(680f, 0f, 0f);

    private Vector3 originalTotalAmmoTextScale;
    private Color originalAmmoColor;


    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        originalTotalAmmoTextScale = totalAmmoText.transform.localScale;
        originalAmmoColor = totalAmmoText.material.color;
    }

    public void Pickup()
    {
        AddAmmo();
    }

    void AddAmmo()
    {
        if (playerGunSelector.ActiveGun.AmmoConfig.CurrentAmmo < playerGunSelector.ActiveGun.AmmoConfig.MaxAmmo)
        {
            IncreaseAmountOfAmmo();
        }
    }

    void IncreaseAmountOfAmmo()
    {
        int maxAmmoPacklAmount = Mathf.Min(totalAmmo, playerGunSelector.ActiveGun.AmmoConfig.MaxAmmo);
        int availableBulletsInAmmoPack = playerGunSelector.ActiveGun.AmmoConfig.MaxAmmo - playerGunSelector.ActiveGun.AmmoConfig.CurrentAmmo;
        int AmountToAdd = Mathf.Min(maxAmmoPacklAmount, availableBulletsInAmmoPack);
        playerGunSelector.ActiveGun.AmmoConfig.CurrentAmmo += AmountToAdd;
        totalAmmo -= AmountToAdd;

        totalAmmoText.DOColor(Color.red, 0.05f)
           .OnComplete(() =>
           {
               totalAmmoText.DOColor(originalAmmoColor, 0.2f);
           });

        totalAmmoText.transform.DOScale(originalTotalAmmoTextScale * 1.5f, 0.05f)
            .OnComplete(() =>
            {
                totalAmmoText.transform.DOScale(originalTotalAmmoTextScale, 0.2f);
            });
    }

    public void Highlight()
    {
        renderer.material.color = Color.yellow;
    }

    public void ResetHighlight()
    {
        renderer.material.color = originalColor;
    }

    public void ShowAmmoPackPanel()
    {
        AmmoPackPanel.transform.DOLocalMove(show, 0.2f);
        headerText.text = headerString;
        mainFirstText.text = mainFirstString;
        totalAmmoText.text = totalAmmo.ToString();
    }

    public void HideAmmoPackPanel()
    {
        AmmoPackPanel.transform.DOLocalMove(hide, 0.2f);
        headerText.text = "";
        mainFirstText.text = "";
        totalAmmoText.text = "";
    }
}
