using DG.Tweening;
using FPS.Guns.Demo;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class AmmoPack : MonoBehaviour, IPickupable
{
    private Color originalColor;
    private new Renderer originalColorRenderer;

    [SerializeField] private GameObject AmmoPackPanel;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI mainFirstText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;
    [SerializeField] private int totalAmmo = 100;

    [SerializeField] private Vector3 hide = new(1100f, 0f, 0f);
    [SerializeField] private Vector3 show = new(680f, 0f, 0f);

    private Vector3 originalTotalAmmoTextScale;
    private Color originalAmmoColor;

    [SerializeField] private GameObject ammoPackModel;

    public LocalizedString localizeStringEventAmmoPack;
    public LocalizedString localizeStringEventAvailableamountofAmmo;

    public LocalizedString localizeStringEventPress;
    public LocalizedString localizeStringEventThisGunHaveFullAmmo;
    public LocalizedString localizeStringEventAddedAmmoTo;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
        originalTotalAmmoTextScale = totalAmmoText.transform.localScale;
        originalAmmoColor = totalAmmoText.material.color;
    }

    private void OnLocaleChanged(Locale _)
    {
        if (AmmoPackPanel != null)
        {
            UpdateLocalizedPanelTexts();
        }
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
            NotificationSystem.Instance.ShowNotification(
                localizeStringEventThisGunHaveFullAmmo,
                $"This {PlayerGunSelector.Instance.ActiveGun.Name} have full ammo",
                1f,
                PlayerGunSelector.Instance.ActiveGun.Name
            );
        }
    }

    void IncreaseAmountOfAmmo()
    {
        int maxAmmoPacklAmount = Mathf.Min(totalAmmo, PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo);
        int availableBulletsInAmmoPack = PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo - PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo;
        int AmountToAdd = Mathf.Min(maxAmmoPacklAmount, availableBulletsInAmmoPack);
        PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo += AmountToAdd;
        totalAmmo -= AmountToAdd;

        NotificationSystem.Instance.ShowNotification(
            localizeStringEventAddedAmmoTo,
            $"Added {AmountToAdd} ammo to {PlayerGunSelector.Instance.ActiveGun.Name}",
            1f,
            AmountToAdd,
            PlayerGunSelector.Instance.ActiveGun.Name
        );

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
        originalColorRenderer.material.color = Color.yellow;
    }

    public void ResetHighlight()
    {
        originalColorRenderer.material.color = originalColor;
    }

    public void ShowAmmoPackPanel()
    {
        AmmoPackPanel.transform.DOLocalMove(show, 0.2f);
        UpdateLocalizedPanelTexts();
        totalAmmoText.text = totalAmmo.ToString();

        NotificationSystem.Instance.ShowInfiniteNotification(
            localizeStringEventPress,
            "Press [E] to restore Ammo!"
        );
    }

    private void UpdateLocalizedPanelTexts()
    {
        string localizedHeader = null;
        if (localizeStringEventAmmoPack != null)
        {
            try
            {
                localizedHeader = localizeStringEventAmmoPack.GetLocalizedString();
            }
            catch { }
        }
        headerText.text = localizedHeader;

        string localizedMain = null;
        if (localizeStringEventAvailableamountofAmmo != null)
        {
            try
            {
                localizedMain = localizeStringEventAvailableamountofAmmo.GetLocalizedString();
            }
            catch { }
        }
        mainFirstText.text = localizedMain;
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
