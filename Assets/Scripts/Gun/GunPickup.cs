using FPS.Guns;
using FPS.Guns.Demo;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GunScriptableObject Gun;
    public PlayerGunSelector playerGunSelector;
    public GunSelector gunSelector;

    public Transform player;
    public float activationDistance = 2f;
    public GameObject imageToActivate;
    public bool isImageActivate = false;
    private bool hasShownNotification = false;


    private void Update()
    {
        if (player == null || imageToActivate == null)
        {
            Debug.LogWarning("Player or Image not assigned in the inspector.");
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        imageToActivate.transform.LookAt(player.position);

        if (distance <= activationDistance && hasShownNotification && playerGunSelector.Guns[playerGunSelector.activeGunIndex] == playerGunSelector.Guns[0])
        {
            NotificationSystem.Instance.HideGunNotification();
            hasShownNotification = false;
            isImageActivate = true;
            imageToActivate.SetActive(true);

           
        }
        else if (distance > activationDistance && !hasShownNotification)
        {
            NotificationSystem.Instance.HideGunNotification();
            isImageActivate = false;
            imageToActivate.SetActive(false);

            hasShownNotification = false;
        }


        if (distance <= activationDistance && !hasShownNotification)
        {
            isImageActivate = true;
            imageToActivate.SetActive(true);

            if (playerGunSelector.Guns[playerGunSelector.activeGunIndex] == playerGunSelector.Guns[1])
            {
                NotificationSystem.Instance.ShowGunNotification($"Press [E] to pick up {Gun.Name}");
                hasShownNotification = true;
          
            }

            
        }
        else if (distance > activationDistance && hasShownNotification)
        {
            NotificationSystem.Instance.HideGunNotification();
            isImageActivate = false;
            imageToActivate.SetActive(false);
            
            hasShownNotification = false;
        }

       
    }


    public void PickupGun()
    {
        if (playerGunSelector.Guns[playerGunSelector.activeGunIndex] == playerGunSelector.Guns[1])
        {
            playerGunSelector.SetupNewGun(Gun);
            Destroy(gameObject);
            isImageActivate = false;
            NotificationSystem.Instance.HideGunNotification();
            if (playerGunSelector.Guns.Count >= 2)
            {

                Sprite gunIconTwo = playerGunSelector.Guns[1].GunIcon;
                gunSelector.secondGunIcon.sprite = gunIconTwo;
            }
        }
    }

}
