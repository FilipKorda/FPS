using FPS.Guns.Demo;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IPickupable
{
    public PlayerGunSelector playerGunSelector;

    private Color originalColor;
    private new Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    public void Pickup()
    {
        if (playerGunSelector.Guns[playerGunSelector.activeGunIndex] == playerGunSelector.Guns[0] && playerGunSelector.Guns[playerGunSelector.activeGunIndex] == playerGunSelector.Guns[1])
        {
            AddAmmo();
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

    public void Highlight()
    {
        renderer.material.color = Color.yellow; 
    }

    public void ResetHighlight()
    {
        renderer.material.color = originalColor;
    }
}
