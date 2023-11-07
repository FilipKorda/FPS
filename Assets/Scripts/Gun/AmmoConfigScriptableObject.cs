using UnityEngine;

namespace FPS.Guns
{
    [CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns/Ammo Config", order = 3)]
    public class AmmoConfigScriptableObject : ScriptableObject, System.ICloneable
    {
        public int MaxAmmo = 120;
        public int ClipSize = 30;

        public int CurrentAmmo = 120;
        public int CurrentClipAmmo = 30;

        public float reloadTime = 2.0f;


        public void Reload()
        {
            int maxReloadAmount = Mathf.Min(ClipSize, CurrentAmmo);
            int availableBulletsInCurrentClip = ClipSize - CurrentClipAmmo;
            int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);
            CurrentClipAmmo += reloadAmount;
            CurrentAmmo -= reloadAmount;
        }


        public bool CanReload()
        {
            return CurrentClipAmmo < ClipSize && CurrentAmmo > 0;
        }

        public void AddAmmo(int Amount)
        {
            if (CurrentAmmo + Amount > MaxAmmo)
            {
                CurrentAmmo = MaxAmmo;
            }
            else
            {
                CurrentAmmo += Amount;
            }
        }


        public void AddAmmoFromAmmoPack()
        {
            CurrentAmmo = MaxAmmo;
        }

        public object Clone()
        {
            AmmoConfigScriptableObject config = CreateInstance<AmmoConfigScriptableObject>();
            Utilities.CopyValues(this, config);
            return config;
        }
    }
}