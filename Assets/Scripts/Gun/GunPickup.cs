using FPS.Guns;
using FPS.Guns.Demo;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class GunPickup : MonoBehaviour, IGunPickupable
{
    public GunScriptableObject Gun;
    public GunSelector gunSelector;

    public GameObject imageToActivate;
    public bool isImageActivate = false;

    [Header("New Gun")]
    [SerializeField] private GameObject gunPanel;
    [SerializeField] private TextMeshProUGUI currentGunText;
    [SerializeField] private TextMeshProUGUI dmgNumberText;
    [SerializeField] private Image dmgStatSign;
    [SerializeField] private TextMeshProUGUI frNumberText;
    [SerializeField] private Image frStatSign;
    [SerializeField] private TextMeshProUGUI maNumberText;
    [SerializeField] private Image maStatSign;
    [SerializeField] private TextMeshProUGUI rtNumberText;
    [SerializeField] private Image rtStatSign;
    [SerializeField] private TextMeshProUGUI msNumberText;
    [SerializeField] private Image msStatSign;
    [SerializeField] private TextMeshProUGUI rrNumberText;
    [SerializeField] private Image rrStatSign;

    [Header("Your Gun")]
    [SerializeField] private TextMeshProUGUI yourCurrentGunText;
    [SerializeField] private TextMeshProUGUI yourDmgNumberText;
    [SerializeField] private TextMeshProUGUI yourFrNumberText;
    [SerializeField] private TextMeshProUGUI yourMaNumberText;
    [SerializeField] private TextMeshProUGUI yourRtNumberText;
    [SerializeField] private TextMeshProUGUI yourMsNumberText;
    [SerializeField] private TextMeshProUGUI yourRrNumberText;


    [SerializeField] private Vector3 hide = new(1100f, 0f, 0f);
    [SerializeField] private Vector3 show = new(680f, 0f, 0f);


    [SerializeField] private Sprite triangleUp;
    [SerializeField] private Sprite triangleDown;
    [SerializeField] private Sprite dot;


    public enum ShapeType
    {
        TriangleUp,
        TriangleDown,
        Dot
    }

    public void ChangeSprite(Image image, ShapeType shapeType)
    {
        switch (shapeType)
        {
            case ShapeType.TriangleUp:
                image.sprite = triangleUp;
                break;

            case ShapeType.TriangleDown:
                image.sprite = triangleDown;
                break;

            case ShapeType.Dot:
                image.sprite = dot;
                break;
            default:
                break;
        }
    }
    void ChangeColor(Image image, Color color)
    {
        image.color = color;
    }
    public void UpdateStatSigns()
    {
        UpdateStatSign(dmgStatSign, PlayerGunSelector.Instance.Guns[1].DamageConfig.DamageCurve.constant, Gun.DamageConfig.DamageCurve.constantMax);
        UpdateStatSign(frStatSign, PlayerGunSelector.Instance.Guns[1].ShootConfig.FireRate, Gun.ShootConfig.FireRate);
        UpdateStatSign(maStatSign, PlayerGunSelector.Instance.Guns[1].AmmoConfig.MaxAmmo, Gun.AmmoConfig.MaxAmmo);
        UpdateStatSign(rtStatSign, PlayerGunSelector.Instance.Guns[1].AmmoConfig.reloadTime, Gun.AmmoConfig.reloadTime);
        UpdateStatSign(msStatSign, PlayerGunSelector.Instance.Guns[1].AmmoConfig.CurrentClipAmmo, Gun.AmmoConfig.CurrentClipAmmo);
        UpdateStatSign(rrStatSign, PlayerGunSelector.Instance.Guns[1].ShootConfig.RecoilRecoverySpeed, Gun.ShootConfig.RecoilRecoverySpeed);
    }

    void UpdateStatSign(Image image, float yourGunStat, float newGunStat)
    {
        ShapeType shapeType = CompareValues(yourGunStat, newGunStat);
        Color color = GetColor(yourGunStat, newGunStat);

        ChangeSprite(image, shapeType);
        ChangeColor(image, color);
    }

    ShapeType CompareValues(float yourGunStat, float newGunStat)
    {
        if (yourGunStat < newGunStat)
        {
            return ShapeType.TriangleUp;
        }
        else if (yourGunStat > newGunStat)
        {
            return ShapeType.TriangleDown;
        }
        else
        {
            return ShapeType.Dot;
        }
    }

    Color GetColor(float playerValue, float gunValue)
    {
        if (playerValue < gunValue)
        {
            return Color.green;
        }
        else if (playerValue > gunValue)
        {
            return Color.red;
        }
        else
        {
            return Color.white;
        }
    }

    public void ShowNotification()
    {
        gunPanel.transform.DOLocalMove(show, 0.2f);
        float minDamage = Gun.DamageConfig.DamageCurve.constantMin;
        float maxDamage = Gun.DamageConfig.DamageCurve.constantMax;

        currentGunText.text = Gun.Name;
        dmgNumberText.text = $"{Mathf.CeilToInt(minDamage)}-{Mathf.CeilToInt(maxDamage)}";
        frNumberText.text = $"{Gun.ShootConfig.FireRate} {"s"}";
        maNumberText.text = Gun.AmmoConfig.MaxAmmo.ToString();
        rtNumberText.text = $"{Gun.AmmoConfig.reloadTime} {"s"}";
        msNumberText.text = Gun.AmmoConfig.CurrentClipAmmo.ToString();
        rrNumberText.text = $"{Gun.ShootConfig.RecoilRecoverySpeed} {"s"}";


        if (PlayerGunSelector.Instance.Guns[1].Type == GunType.Glock)
        {
            float maxConstantDamage = PlayerGunSelector.Instance.Guns[1].DamageConfig.DamageCurve.constant;
            yourCurrentGunText.text = PlayerGunSelector.Instance.Guns[1].Name;
            yourDmgNumberText.text = $"{Mathf.CeilToInt(maxConstantDamage)}";
            yourFrNumberText.text = $"{PlayerGunSelector.Instance.Guns[1].ShootConfig.FireRate} {"s"}";
            yourMaNumberText.text = PlayerGunSelector.Instance.Guns[1].AmmoConfig.MaxAmmo.ToString();
            yourRtNumberText.text = $"{PlayerGunSelector.Instance.Guns[1].AmmoConfig.reloadTime} {"s"}";
            yourMsNumberText.text = PlayerGunSelector.Instance.Guns[1].AmmoConfig.CurrentClipAmmo.ToString();
            yourRrNumberText.text = $"{PlayerGunSelector.Instance.Guns[1].ShootConfig.RecoilRecoverySpeed} {"s"}";
        }
        else
        {
            float minDamageSecond = PlayerGunSelector.Instance.Guns[1].DamageConfig.DamageCurve.constantMin;
            float maxDamageSecond = PlayerGunSelector.Instance.Guns[1].DamageConfig.DamageCurve.constantMax;
            yourCurrentGunText.text = PlayerGunSelector.Instance.Guns[1].Name;
            yourDmgNumberText.text = $"{Mathf.CeilToInt(minDamageSecond)}-{Mathf.CeilToInt(maxDamageSecond)}";
            yourFrNumberText.text = $"{PlayerGunSelector.Instance.Guns[1].ShootConfig.FireRate} {"s"}";
            yourMaNumberText.text = PlayerGunSelector.Instance.Guns[1].AmmoConfig.MaxAmmo.ToString();
            yourRtNumberText.text = $"{PlayerGunSelector.Instance.Guns[1].AmmoConfig.reloadTime} {"s"}";
            yourMsNumberText.text = PlayerGunSelector.Instance.Guns[1].AmmoConfig.CurrentClipAmmo.ToString();
            yourRrNumberText.text = $"{PlayerGunSelector.Instance.Guns[1].ShootConfig.RecoilRecoverySpeed} {"s"}";
        }

        UpdateStatSigns();

        imageToActivate.SetActive(true);
        isImageActivate = true;
       // NotificationSystem.Instance.ShowInfiniteNotification($"Press [E] to pick up {Gun.Name}");

    }

    public void HideNotification()
    {
        if (imageToActivate != null)
        {
            gunPanel.transform.DOLocalMove(hide, 0.2f);

            currentGunText.text = "";
            dmgNumberText.text = "";
            frNumberText.text = "";
            maNumberText.text = "";
            rtNumberText.text = "";
            msNumberText.text = "";
            rrNumberText.text = "";

            yourCurrentGunText.text = "";
            yourDmgNumberText.text = "";
            yourFrNumberText.text = "";
            yourMaNumberText.text = "";
            yourRtNumberText.text = "";
            yourMsNumberText.text = "";

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
