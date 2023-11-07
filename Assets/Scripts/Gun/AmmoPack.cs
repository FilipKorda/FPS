using FPS.Guns.Demo;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IPickupable
{
    public PlayerGunSelector playerGunSelector;

    private Color originalColor;
    private new Renderer renderer;

    public int totalAmmo = 100;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    public void Pickup()
    {
        AddAmmo();
    }

    void AddAmmo()
    {
        if (playerGunSelector.ActiveGun.AmmoConfig.CurrentAmmo < playerGunSelector.ActiveGun.AmmoConfig.MaxAmmo)
        {

            playerGunSelector.ActiveGun.AmmoConfig.AddAmmoFromAmmoPack();
        }
    }

    void IncreaseAmountOfAmmo()
    {
        int maxReloadAmount = Mathf.Min(totalAmmo, playerGunSelector.ActiveGun.AmmoConfig.MaxAmmo);
        int availableBulletsInCurrentClip = playerGunSelector.ActiveGun.AmmoConfig.MaxAmmo - totalAmmo;
        int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);
        playerGunSelector.ActiveGun.AmmoConfig.MaxAmmo += reloadAmount;
    }


    public void Highlight()
    {
        renderer.material.color = Color.yellow;
    }

    public void ResetHighlight()
    {
        renderer.material.color = originalColor;
    }
}
