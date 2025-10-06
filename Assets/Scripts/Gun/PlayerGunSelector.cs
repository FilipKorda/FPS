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

        public float normalFOV = 60f;
        public float zoomFOV = 30f;
        public float zoomSpeed = 5f;
        private bool isZoomed = false;
        private Vector3 originalWeaponPosition = new(0.35f, -0.3f, 0.6f);
        public Vector3 glockZoomedPosition = new(0f, -0.14f, 0.33f);
        public Vector3 m4a1ZoomedPosition = new(0f, -0.1f, 0.3f);
        public Vector3 uziSilencerZoomedPosition = new(0f, -0.155f, 0.4f);

        [Header("Weapon Draw")]
        [SerializeField] private float drawDuration = 0.15f;
        [SerializeField] private float drawStartYOffset = -0.5f;
        private Coroutine drawCoroutine;

        private readonly Dictionary<Transform, Coroutine> childDrawCoroutines = new();
        private readonly HashSet<Transform> seenChildren = new();

        private void Start()
        {
            ActiveBaseGun = GetGunOfType(GunType.M4A1);
            ActiveGun = GetCachedGun(ActiveBaseGun);
            ActiveGun.Spawn(GunParent, this, Camera);

            PlayDrawAnimation();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                isZoomed = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                isZoomed = false;
            }

            UpdateZoom();

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

        void UpdateZoom()
        {
            if (!DialogueManager.Instance.isTalking)
            {
                float targetFOV = isZoomed ? zoomFOV : cameraFovSettings.ClampedValue;
                Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

                float lerpValue = isZoomed ? 1f : 0f;

                if (ActiveGun.Type == GunType.Glock)
                {
                    GunParent.localPosition = Vector3.Lerp(originalWeaponPosition, glockZoomedPosition, lerpValue);
                }

                if (ActiveGun.Type == GunType.M4A1)
                {
                    GunParent.localPosition = Vector3.Lerp(originalWeaponPosition, m4a1ZoomedPosition, lerpValue);
                }

                if (ActiveGun.Type == GunType.UziSilencer)
                {
                    GunParent.localPosition = Vector3.Lerp(originalWeaponPosition, uziSilencerZoomedPosition, lerpValue);
                }
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