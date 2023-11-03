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

        private void Start()
        {
            ActiveBaseGun = GetGunOfType(GunType.M4A1);
            ActiveGun = GetCachedGun(ActiveBaseGun);
            ActiveGun.Spawn(GunParent, this, Camera);
        }

        private void Update()
        {           
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