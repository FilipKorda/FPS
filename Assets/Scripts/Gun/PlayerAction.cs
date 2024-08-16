using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.Guns.Demo
{
    [DisallowMultipleComponent]
    public class PlayerAction : MonoBehaviour
    {
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
                PlayerGunSelector.Instance.ActiveGun.Tick(
                    Application.isFocused && Input.GetMouseButton(0)
                    && PlayerGunSelector.Instance.ActiveGun != null
                );

                if (ShouldManualReload() && Time.timeScale == 1)
                {
                    PlayerGunSelector.Instance.ActiveGun.StartReloading();
                    IsReloading = true;
                    StartCoroutine(ChangeSliderValueOverTime(PlayerGunSelector.Instance.ActiveGun.AmmoConfig.reloadTime));
                    reloadTimer = PlayerGunSelector.Instance.ActiveGun.AmmoConfig.reloadTime;
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
            Vector3 gunTipPoint = PlayerGunSelector.Instance.ActiveGun.GetRaycastOrigin();
            Vector3 forward;
            if (PlayerGunSelector.Instance.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
            {
                forward = PlayerGunSelector.Instance.ActiveGun.GetGunForward();
            }
            else
            {
                forward = PlayerGunSelector.Instance.Camera.transform.forward;
            }

            Vector3 hitPoint = gunTipPoint + forward * 10;

            if (Physics.Raycast(gunTipPoint, forward, out RaycastHit hit, float.MaxValue, PlayerGunSelector.Instance.ActiveGun.ShootConfig.HitMask))
            {
                hitPoint = hit.point;
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Crosshair.color = Color.red;
                }
                else
                {
                    Crosshair.color = Color.black;
                }
            }

            if (PlayerGunSelector.Instance.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
            {
                Vector3 screenSpaceLocation = PlayerGunSelector.Instance.Camera.WorldToScreenPoint(hitPoint);

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
                && PlayerGunSelector.Instance.ActiveGun.CanReload();
        }

        private void EndReload()
        {
            PlayerGunSelector.Instance.ActiveGun.EndReload();
            IsReloading = false;
        }
    }
}