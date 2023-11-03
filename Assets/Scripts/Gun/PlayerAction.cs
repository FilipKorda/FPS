using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.Guns.Demo
{
    [DisallowMultipleComponent]
    public class PlayerAction : MonoBehaviour
    {
        public PlayerGunSelector GunSelector;
        [SerializeField]
        private Image Crosshair;
        public bool IsReloading;

        public Slider slider;

        private float reloadTimer = 0f;

        private void Update()
        {
            if (IsReloading)
            {
                reloadTimer -= Time.deltaTime;

                if (reloadTimer <= 0)
                {
                    EndReload();
                }
            }
            else
            {
                GunSelector.ActiveGun.Tick(
                    Application.isFocused && Input.GetMouseButton(0)
                    && GunSelector.ActiveGun != null
                );

                if (ShouldManualReload())
                {
                    GunSelector.ActiveGun.StartReloading();
                    IsReloading = true;
                    StartCoroutine(ChangeSliderValueOverTime(GunSelector.ActiveGun.AmmoConfig.reloadTime));
                    reloadTimer = GunSelector.ActiveGun.AmmoConfig.reloadTime;
                }
            }
            UpdateCrosshair();
        }

        private IEnumerator ChangeSliderValueOverTime(float time)
        {
            slider.gameObject.SetActive(true);
            float elapsedTime = 0;
            float startValue = slider.value;
            float endValue = 1.0f;

            while (elapsedTime < time)
            {
                slider.value = Mathf.Lerp(startValue, endValue, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            slider.value = endValue;
            ResetSliderToZero();
        }

        private void ResetSliderToZero()
        {
            slider.gameObject.SetActive(false);
            slider.value = 0;
        }

        private void UpdateCrosshair()
        {
            Vector3 gunTipPoint = GunSelector.ActiveGun.GetRaycastOrigin();
            Vector3 forward;
            if (GunSelector.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
            {
                forward = GunSelector.ActiveGun.GetGunForward();
            }
            else
            {
                forward = GunSelector.Camera.transform.forward;
            }

            Vector3 hitPoint = gunTipPoint + forward * 10;

            if (Physics.Raycast(gunTipPoint, forward, out RaycastHit hit, float.MaxValue, GunSelector.ActiveGun.ShootConfig.HitMask))
            {
                hitPoint = hit.point;
            }

            if (GunSelector.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
            {
                Vector3 screenSpaceLocation = GunSelector.Camera.WorldToScreenPoint(hitPoint);

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)Crosshair.transform.parent,
                    screenSpaceLocation,
                    null,
                    out Vector2 localPosition))
                {
                    Crosshair.rectTransform.anchoredPosition = localPosition;
                }
                else
                {
                    Crosshair.rectTransform.anchoredPosition = Vector2.zero;
                }
            }
            else
            {
                Crosshair.rectTransform.anchoredPosition = Vector2.zero;
            }
        }

        private bool ShouldManualReload()
        {
            return !IsReloading
                && Input.GetKeyUp(KeyCode.R)
                && GunSelector.ActiveGun.CanReload();
        }

        private void EndReload()
        {
            GunSelector.ActiveGun.EndReload();
            IsReloading = false;
        }
    }
}