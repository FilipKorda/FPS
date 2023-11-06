using FPS.Guns.Demo;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IPickupable
{
    public PlayerGunSelector playerGunSelector;

    public void Pickup()
    {
        if (playerGunSelector.Guns[playerGunSelector.activeGunIndex] == playerGunSelector.Guns[0] && playerGunSelector.Guns[playerGunSelector.activeGunIndex] == playerGunSelector.Guns[1])
        {
            AddAmmo();
            NotificationSystem.Instance.ShowNotification("Dostajesz ammo xddddd", 2f);
        }
    }

    void AddAmmo()
    {
        foreach (var gun in playerGunSelector.Guns)
        {
            gun.AmmoConfig.CurrentAmmo = gun.AmmoConfig.MaxAmmo;
        }

        Debug.Log("You get ammo");
    }

}
