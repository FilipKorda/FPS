using System.Collections.Generic;
using UnityEngine;

namespace FPS.Guns.Demo
{
    [DisallowMultipleComponent]
    public class PlayerGunSelector : MonoBehaviour
    {
        public static PlayerGunSelector Instance { get; private set; }

        public Camera Camera;
        [field: SerializeField] public GunType Gun { get; private set; }

        [SerializeField] private Transform GunParent;
        [field: SerializeField] public List<GunScriptableObject> Guns { get; private set; }
        public int activeGunIndex = 0;

        [Space][Header("Runtime Filled")] public GunScriptableObject ActiveGun;
        [field: SerializeField] public GunScriptableObject ActiveBaseGun { get; private set; }

        [SerializeField] private bool InitializeOnStart = false;
        [SerializeField] private GunSelector GunSelector;
        [SerializeField] private PlayerAction PlayerAction;
        [SerializeField] private CameraFovSettings cameraFovSettings;

        private readonly Dictionary<GunType, GunScriptableObject> gunCache = new();

        [Header("FOV / Zoom")]
        [Tooltip("Mno¿nik FOV podczas celowania wzglêdem bazowego FOV z opcji.")]
        [SerializeField, Range(0.1f, 1f)] private float zoomFovMultiplier = 0.5f;
        [SerializeField] private float minZoomFov = 20f;
        [SerializeField] private float maxZoomFov = 120f;

        private const float defaultBaseFov = 60f;
        public bool isZoomed = false;

        private float cachedBaseFov;
        private float targetFovHip;
        private float targetFovAds;

        [Header("FOV Smoothing")]
        [SerializeField] private bool smoothZoom = true;
        [SerializeField, Range(0.02f, 0.3f)] private float fovSmoothTime = 0.08f;
        [SerializeField, Range(0.001f, 0.2f)] private float fovSnapEpsilon = 0.03f;
        private float fovVelocity;
        private float currentFov; 

        private Vector3 originalWeaponPosition = new(0.35f, -0.3f, 0.6f);
        public Vector3 glockZoomedPosition = new(0f, -0.14f, 0.33f);
        public Vector3 m4a1ZoomedPosition = new(0f, -0.1f, 0.3f);
        public Vector3 uziSilencerZoomedPosition = new(0f, -0.155f, 0.4f);

        [SerializeField] private float drawDuration = 0.15f;
        [SerializeField] private float drawStartYOffset = -0.5f;
        private Coroutine drawCoroutine;

        private readonly Dictionary<Transform, Coroutine> childDrawCoroutines = new();
        private readonly HashSet<Transform> seenChildren = new();

        [Header("Zoom effects")]
        [SerializeField, Range(0f, 1f)] private float zoomSpeedMultiplier = 0.2f; 
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CameraHeadBob cameraHeadBob;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            ActiveBaseGun = GetGunOfType(GunType.M4A1);
            ActiveGun = GetCachedGun(ActiveBaseGun);
            ActiveGun.Spawn(GunParent, this, Camera);

            if (GunParent != null)
            {
                originalWeaponPosition = GunParent.localPosition;
            }

            RecomputeFovTargets();

            currentFov = targetFovHip;
            if (Camera != null)
            {
                Camera.fieldOfView = currentFov;
            }

            PlayDrawAnimation();
        }

        private float GetBaseFov()
        {
            if (cameraFovSettings != null)
                return cameraFovSettings.ClampedValue;

            if (PlayerPrefs.HasKey("FOVValue"))
                return PlayerPrefs.GetFloat("FOVValue");

            return defaultBaseFov;
        }

        private void RecomputeFovTargets()
        {
            cachedBaseFov = Mathf.Clamp(GetBaseFov(), minZoomFov, maxZoomFov);
            targetFovHip = cachedBaseFov;
            targetFovAds = Mathf.Clamp(cachedBaseFov * zoomFovMultiplier, minZoomFov, maxZoomFov);
        }

        private void OnDisable()
        {
            ApplyZoomEffects(false);
        }

        private void Update()
        {
            bool rmb = Input.GetMouseButton(1);
            if (rmb != isZoomed)
            {
                isZoomed = rmb;
                ApplyZoomEffects(isZoomed);

                RecomputeFovTargets();
                fovVelocity = 0f;
            }

            if (!isZoomed)
            {
                float baseNow = Mathf.Clamp(GetBaseFov(), minZoomFov, maxZoomFov);
                if (!Mathf.Approximately(baseNow, cachedBaseFov))
                {
                    cachedBaseFov = baseNow;
                    targetFovHip = cachedBaseFov;
                    targetFovAds = Mathf.Clamp(cachedBaseFov * zoomFovMultiplier, minZoomFov, maxZoomFov);

                    if (!smoothZoom)
                    {
                        currentFov = targetFovHip;
                    }
                }
            }

            CheckAndAnimateNewChildren();

            if (!PlayerAction.IsReloading && !isZoomed)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && activeGunIndex != 0)
                {
                    activeGunIndex = 0;
                    SwitchGunModel(0);
                    GunSelector.SwitchGunOnUI(0);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && activeGunIndex != 1)
                {
                    activeGunIndex = 1;
                    SwitchGunModel(1);
                    GunSelector.SwitchGunOnUI(1);
                }
            }

            if (PlayerSingleton.Instance != null && !PlayerSingleton.Instance.canShoot)
            {
                return;
            }
        }

        private void LateUpdate()
        {
            UpdateZoom();
        }

        private void ApplyZoomEffects(bool zoomOn)
        {
            if (playerController != null)
            {
                playerController.SetExternalSpeedMultiplier(zoomOn ? zoomSpeedMultiplier : 1f);
            }
            if (cameraHeadBob != null)
            {
                cameraHeadBob.SetZooming(zoomOn);
            }
        }

        void UpdateZoom()
        {
            if (Camera == null) return;
            if (DialogueManager.Instance != null && DialogueManager.Instance.isTalking)
                return;

            float targetFOV = isZoomed ? targetFovAds : targetFovHip;

            if (smoothZoom)
            {
                currentFov = Mathf.SmoothDamp(
                    currentFov,
                    targetFOV,
                    ref fovVelocity,
                    fovSmoothTime,
                    Mathf.Infinity,
                    Time.deltaTime
                );

                if (Mathf.Abs(currentFov - targetFOV) <= fovSnapEpsilon)
                {
                    currentFov = targetFOV;
                    fovVelocity = 0f;
                }
            }
            else
            {
                currentFov = targetFOV;
            }

            Camera.fieldOfView = currentFov;

            if (GunParent != null && ActiveGun != null)
            {
                Vector3 targetPos = originalWeaponPosition;

                if (isZoomed)
                {
                    if (ActiveGun.Type == GunType.Glock)
                        targetPos = glockZoomedPosition;
                    else if (ActiveGun.Type == GunType.M4A1)
                        targetPos = m4a1ZoomedPosition;
                    else if (ActiveGun.Type == GunType.UziSilencer)
                        targetPos = uziSilencerZoomedPosition;
                }

                GunParent.localPosition = targetPos;
            }
        }

        public void SetupNewGun(GunScriptableObject newGun)
        {
            if (newGun != null)
            {
                ActiveGun.Despawn();
                ActiveBaseGun = newGun;
                ActiveGun = GetCachedGun(ActiveBaseGun);
                ActiveGun.Spawn(GunParent, this, Camera);

                if (GunParent != null)
                    originalWeaponPosition = GunParent.localPosition;

                RecomputeFovTargets();
                
                fovVelocity = 0f;

                PlayDrawAnimation();

                if (Guns.Count > 1)
                {
                    Guns.RemoveAt(Guns.Count - 1);
                }

                Guns.Add(newGun);
            }
        }

        private void SwitchGunModel(int gunIndex)
        {
            if (gunIndex < 0 || gunIndex >= Guns.Count)
            {
                Debug.LogError($"Invalid gun index: {gunIndex}");
                return;
            }

            GunScriptableObject newGun = Guns[gunIndex];

            ActiveGun.Despawn();
            ActiveBaseGun = newGun;
            ActiveGun = GetCachedGun(ActiveBaseGun);
            ActiveGun.Spawn(GunParent, this, Camera);

            if (GunParent != null)
                originalWeaponPosition = GunParent.localPosition;

            RecomputeFovTargets();
            fovVelocity = 0f;

            PlayDrawAnimation();
        }

        public void DespawnActiveGun()
        {
            if (ActiveGun != null)
            {
                ActiveGun.Despawn();
            }

            Destroy(ActiveGun);
        }

        private GunScriptableObject GetGunOfType(GunType gunType)
        {
            return Guns.Find(gun => gun.Type == gunType);
        }

        private GunScriptableObject GetCachedGun(GunScriptableObject gun)
        {
            if (!gunCache.ContainsKey(gun.Type))
            {
                gunCache[gun.Type] = gun.Clone() as GunScriptableObject;
            }
            return gunCache[gun.Type];
        }

        public void PlayDrawAnimationInIntro()
        {
            PlayDrawAnimation();
        }

        private void PlayDrawAnimation()
        {
            if (GunParent == null) return;

            CheckAndAnimateNewChildren(forceAnimateExisting: true);
        }

        private void CheckAndAnimateNewChildren(bool forceAnimateExisting = false)
        {
            if (GunParent == null) return;

            for (int i = 0; i < GunParent.childCount; i++)
            {
                var child = GunParent.GetChild(i);

                if (forceAnimateExisting || !seenChildren.Contains(child))
                {
                    seenChildren.Add(child);
                    StartChildDrawAnimation(child);
                }
            }

            var toRemove = new List<Transform>();
            foreach (var tr in seenChildren)
            {
                if (tr == null || tr.parent != GunParent)
                {
                    toRemove.Add(tr);
                }
            }
            foreach (var tr in toRemove)
            {
                seenChildren.Remove(tr);
                if (childDrawCoroutines.TryGetValue(tr, out var co) && co != null)
                {
                    StopCoroutine(co);
                }
                childDrawCoroutines.Remove(tr);
            }
        }

        private void StartChildDrawAnimation(Transform child)
        {
            if (child == null) return;

            if (childDrawCoroutines.TryGetValue(child, out var running) && running != null)
            {
                StopCoroutine(running);
            }

            var co = StartCoroutine(DrawRoutine(child));
            childDrawCoroutines[child] = co;
        }

        private System.Collections.IEnumerator DrawRoutine(Transform weapon)
        {
            Vector3 end = weapon.localPosition;
            float startY = drawStartYOffset;
            float endY = 0f;

            weapon.localPosition = new Vector3(end.x, startY, end.z);

            float t = 0f;
            float duration = Mathf.Max(0.0001f, drawDuration);

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                float eased = 1f - Mathf.Pow(1f - Mathf.Clamp01(t), 3f);
                float y = Mathf.LerpUnclamped(startY, endY, eased);
                weapon.localPosition = new Vector3(end.x, y, end.z);
                yield return null;
            }

            weapon.localPosition = new Vector3(end.x, endY, end.z);
        }
    }
}