using TMPro;
using UnityEngine;
using DG.Tweening;

namespace FPS.Guns.Demo
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AmmoDisplayer : MonoBehaviour
    {
        [SerializeField]
        private PlayerGunSelector playerGunSelector;
        private TextMeshProUGUI AmmoText;

        public bool isCurrentAmmo;
        private Vector3 originalTotalAmmoTextScale;
        private Color originalAmmoColor;


        private void Start()
        {
            originalTotalAmmoTextScale = AmmoText.transform.localScale;
            originalAmmoColor = AmmoText.material.color;
        }

        private void Awake()
        {
            AmmoText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (isCurrentAmmo)
            {
                AmmoText.SetText($"{playerGunSelector.ActiveGun.AmmoConfig.CurrentAmmo}");
            }
            else
            {
                AmmoText.SetText($"{playerGunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo}");
            }

        }

        public void AmmoChanged()
        {
            AmmoText.SetText($"{playerGunSelector.ActiveGun.AmmoConfig.CurrentAmmo}");

            AmmoText.DOColor(Color.green, 0.05f)
                .OnComplete(() =>
                {
                    AmmoText.DOColor(originalAmmoColor, 0.2f);
                });

            AmmoText.transform.DOScale(originalTotalAmmoTextScale * 1.5f, 0.05f)
                .OnComplete(() =>
                {
                    AmmoText.transform.DOScale(originalTotalAmmoTextScale, 0.2f);
                });



        }
    }
}