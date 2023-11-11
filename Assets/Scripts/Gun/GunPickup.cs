using FPS.Guns;
using FPS.Guns.Demo;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GunPickup : MonoBehaviour, IGunPickupable
{
    public GunScriptableObject Gun;
    public GunSelector gunSelector;

    public GameObject imageToActivate;
    public bool isImageActivate = false;


    [SerializeField] private GameObject gunPanel;
    [SerializeField] private TextMeshProUGUI currentGunText;
    [SerializeField] private TextMeshProUGUI dmgNumberText;
    [SerializeField] private TextMeshProUGUI frNumberText;
    [SerializeField] private TextMeshProUGUI taNumberText;
    [SerializeField] private TextMeshProUGUI rtNumberText;
    [SerializeField] private TextMeshProUGUI msNumberText;
    [SerializeField] private TextMeshProUGUI rrNumberText;

    [SerializeField] private Vector3 hide = new(1100f, 0f, 0f);
    [SerializeField] private Vector3 show = new(680f, 0f, 0f);


    public void ShowNotification()
    {
        gunPanel.transform.DOLocalMove(show, 0.2f);

        currentGunText.text = Gun.Name;

        float minDamage = Gun.DamageConfig.DamageCurve.constantMin;
        float maxDamage = Gun.DamageConfig.DamageCurve.constantMax;

        dmgNumberText.text = $"{Mathf.CeilToInt(minDamage)}-{Mathf.CeilToInt(maxDamage)}";

        frNumberText.text = Gun.ShootConfig.FireRate.ToString();
        taNumberText.text = Gun.AmmoConfig.MaxAmmo.ToString();
        rtNumberText.text = Gun.AmmoConfig.reloadTime.ToString();
        msNumberText.text = Gun.AmmoConfig.CurrentClipAmmo.ToString();
        rrNumberText.text = Gun.ShootConfig.RecoilRecoverySpeed.ToString();

        imageToActivate.SetActive(true);
        isImageActivate = true;
        NotificationSystem.Instance.ShowInfiniteNotification($"Press [E] to pick up {Gun.Name}");

    }

    public void HideNotification()
    {
        if (imageToActivate != null)
        {
            gunPanel.transform.DOLocalMove(hide, 0.2f);

            currentGunText.text = "";
            dmgNumberText.text = "";
            frNumberText.text = "";
            taNumberText.text = "";
            rtNumberText.text = "";
            msNumberText.text = "";
            rrNumberText.text = "";

            imageToActivate.SetActive(false);
            isImageActivate = false;
            NotificationSystem.Instance.HideInfiniteNotification();
        }

    }

    public void PickupGun()
    {
        if (PlayerGunSelector.Instance.Guns[PlayerGunSelector.Instance.activeGunIndex] == PlayerGunSelector.Instance.Guns[1])
        {
            PlayerGunSelector.Instance.SetupNewGun(Gun);
            Destroy(gameObject);
            HideNotification();
            if (PlayerGunSelector.Instance.Guns.Count >= 2)
            {
                Sprite gunIconTwo = PlayerGunSelector.Instance.Guns[1].GunIcon;
                gunSelector.secondGunIcon.sprite = gunIconTwo;
            }
        }
    }
}
