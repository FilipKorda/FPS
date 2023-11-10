using FPS.Guns.Demo;
using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    [SerializeField]
    private int amountOfAmmo = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo < PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo)
            {
                IncreaseAmountOfAmmo();
                Destroy(gameObject);
            }

        }
    }

    void IncreaseAmountOfAmmo()
    {
        int maxAmmoAmount = Mathf.Min(amountOfAmmo, PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo);
        int availableBulletsInAmmo = PlayerGunSelector.Instance.ActiveGun.AmmoConfig.MaxAmmo - PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo;

        int amountToAdd = Mathf.Min(maxAmmoAmount, availableBulletsInAmmo);

        PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentAmmo += amountToAdd;

        NotificationSystem.Instance.ShowNotification($"Add {amountToAdd} ammo to {PlayerGunSelector.Instance.ActiveGun.Name}", 1.0f);


        AmmoDisplayer.Instance.AmmoChanged();


    }

}
