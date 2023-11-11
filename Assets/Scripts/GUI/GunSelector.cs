using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FPS.Guns.Demo
{
    public class GunSelector : MonoBehaviour
    {
        [Header("Current Gun Selected")]
        [Space(5)]
        [SerializeField]
        private Image HLFirstGunImage;
        [SerializeField]
        private Image HLSecondGunImage;
        [Header("Current Gun Icon Selected")]
        [Space(5)]
        [SerializeField]
        private Image firstGunIcon;
        public Image secondGunIcon;

        private Vector3 initialPistolScale;
        private Vector3 initialMachineGunScale;
        private readonly float durationFade = 0.2f;
        private readonly float scaleDuration = 0.07f;
        private readonly float scaleFactor = 1.05f;
        private Color startFirstGunColor;
        private Color startSecondGunColor;


        private void Start()
        {
            DoOnStart();
        }

        private void Update()
        {
            if (PlayerGunSelector.Instance.Guns.Count >= 1)
            {
                Sprite gunIconOne = PlayerGunSelector.Instance.Guns[0].GunIcon;
                firstGunIcon.sprite = gunIconOne;

                if (PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo == 0)
                {
                    firstGunIcon.color = Color.red;
                }
                else
                {
                    firstGunIcon.color = Color.white;
                }
            }

            if (PlayerGunSelector.Instance.Guns.Count >= 2)
            {
                Sprite gunIconTwo = PlayerGunSelector.Instance.Guns[1].GunIcon;
                secondGunIcon.sprite = gunIconTwo;
                if (PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo == 0)
                {
                    secondGunIcon.color = Color.red;
                }
                else
                {
                    secondGunIcon.color = Color.white;
                }
            }
        }

        public void SwitchGunOnUI(int gunIndex)
        {
            if (gunIndex >= 0 && gunIndex < PlayerGunSelector.Instance.Guns.Count)
            {
                if (PlayerGunSelector.Instance.Guns[PlayerGunSelector.Instance.activeGunIndex] == PlayerGunSelector.Instance.Guns[0])
                {
                    HLFirstGunImage.DOFade(0.2f, durationFade);
                    HLSecondGunImage.DOFade(0f, durationFade);

                    AnimateScale(HLFirstGunImage, initialPistolScale);
                    Sprite gunIconOne = PlayerGunSelector.Instance.Guns[gunIndex].GunIcon;

                    firstGunIcon.sprite = gunIconOne;

                }
                else
                {
                    HLSecondGunImage.DOFade(0.2f, durationFade);
                    HLFirstGunImage.DOFade(0f, durationFade);

                    AnimateScale(HLSecondGunImage, initialMachineGunScale);

                    Sprite gunIconTwo = PlayerGunSelector.Instance.Guns[gunIndex].GunIcon;

                    secondGunIcon.sprite = gunIconTwo;
                }
            }
        }

        private void AnimateScale(Image obj, Vector3 initialScale)
        {
            obj.transform.DOScale(initialScale * scaleFactor, scaleDuration)
                .OnComplete(() => obj.transform.DOScale(initialScale, 0.0f));
        }

        private void DoOnStart()
        {
            //Size
            initialPistolScale = HLFirstGunImage.transform.localScale;
            initialMachineGunScale = HLSecondGunImage.transform.localScale;

            //Alfa Color On Start
            startFirstGunColor = HLFirstGunImage.color;
            startFirstGunColor.a = 0.2f;
            HLFirstGunImage.color = startFirstGunColor;

            startSecondGunColor = HLSecondGunImage.color;
            startSecondGunColor.a = 0f;
            HLSecondGunImage.color = startSecondGunColor;

            //Show Icons on UI
            Sprite initialFirstGunIcon = PlayerGunSelector.Instance.Guns[0].GunIcon;
            firstGunIcon.sprite = initialFirstGunIcon;

            Sprite initialSecondGunIcon = PlayerGunSelector.Instance.Guns[1].GunIcon;
            secondGunIcon.sprite = initialSecondGunIcon;
        }
    }

}
