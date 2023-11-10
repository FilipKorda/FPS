using System.Collections.Generic;
using UnityEngine;

namespace FPS.Guns.Demo
{
    [DisallowMultipleComponent]
    public class PlayerGunSelector : MonoBehaviour
    {
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

        private readonly Dictionary<GunType, GunScriptableObject> gunCache = new();

        public float normalFOV = 60f;
        public float zoomFOV = 30f;
        public float zoomSpeed = 5f;
        private bool isZoomed = false;
        private Vector3 originalWeaponPosition = new(0.35f, -0.3f, 0.6f);
        public Vector3 glockZoomedPosition = new(0f, -0.14f, 0.33f);
        public Vector3 m4a1ZoomedPosition = new(0f, -0.155f, 0.4f);
        public Vector3 uziSilencerZoomedPosition = new(0f, -0.155f, 0.4f);

        private void Start()
        {
            ActiveBaseGun = GetGunOfType(GunType.M4A1);
            ActiveGun = GetCachedGun(ActiveBaseGun);
            ActiveGun.Spawn(GunParent, this, Camera);
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



            if (!PlayerAction.IsReloading)
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
        }

        void UpdateZoom()
        {
            float targetFOV = isZoomed ? zoomFOV : normalFOV;
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

            float lerpValue = isZoomed ? 1f : 0f;

            if(ActiveGun.Type == GunType.Glock)
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




        public void SetupNewGun(GunScriptableObject newGun)
        {
            if (newGun != null)
            {
                ActiveGun.Despawn();
                ActiveBaseGun = newGun;
                ActiveGun = GetCachedGun(ActiveBaseGun);
                ActiveGun.Spawn(GunParent, this, Camera);

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

    }
}