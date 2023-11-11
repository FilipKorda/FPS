using DG.Tweening;
using FPS.Guns.Demo;
using TMPro;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IPickupable
{
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

    [SerializeField] private GameObject ammoPackModel;


    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        originalTotalAmmoTextScale = totalAmmoText.transform.localScale;
        originalAmmoColor = totalAmmoText.material.color;
    }

    public void PickupAmmo()
    {
        AddAmmo();
    }

    void AddAmmo()
    {
        if (PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo < PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo)
        {
            IncreaseAmountOfAmmo();
            ShakeAmmoPackModel();

            AmmoDisplayer.Instance.AmmoChanged();

        }
        else
        {
            NotificationSystem.Instance.ShowNotification($"This {PlayerGunSelector.Instance.ActiveGun.Name} have full ammo", 1f);
        }
    }

    void IncreaseAmountOfAmmo()
    {
        int maxAmmoPacklAmount = Mathf.Min(totalAmmo, PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo);
        int availableBulletsInAmmoPack = PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo - PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo;
        int AmountToAdd = Mathf.Min(maxAmmoPacklAmount, availableBulletsInAmmoPack);
        PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo += AmountToAdd;
        totalAmmo -= AmountToAdd;

        NotificationSystem.Instance.ShowNotification($"Added {AmountToAdd} ammo to {PlayerGunSelector.Instance.ActiveGun.Name}", 1f);

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

    void ShakeAmmoPackModel()
    {
        float duration = 0.1f;
        float strength = 0.05f;
        int vibrato = 1;

        ammoPackModel.transform.DOShakePosition(duration, strength, vibrato);
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
        NotificationSystem.Instance.ShowInfiniteNotification("Press [E] to restore Ammo!");
    }

    public void HideAmmoPackPanel()
    {
        AmmoPackPanel.transform.DOLocalMove(hide, 0.2f);
        headerText.text = "";
        mainFirstText.text = "";
        totalAmmoText.text = "";
        NotificationSystem.Instance.HideInfiniteNotification();
    }
}
