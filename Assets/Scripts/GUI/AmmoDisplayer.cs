using TMPro;
using UnityEngine;
using DG.Tweening;

namespace FPS.Guns.Demo
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AmmoDisplayer : MonoBehaviour
    {
        public static AmmoDisplayer Instance { get; private set; }

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
            Instance = this;
            AmmoText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (isCurrentAmmo)
            {
                AmmoText.SetText($"{PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo}");
            }
            else
            {
                AmmoText.SetText($"{PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentClipAmmo}");
            }

        }

        public void AmmoChanged()
        {

            AmmoText.SetText($"{PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo}");


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