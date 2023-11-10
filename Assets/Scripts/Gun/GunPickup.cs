using FPS.Guns;
using FPS.Guns.Demo;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GunScriptableObject Gun;
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

        if (distance <= activationDistance && hasShownNotification && PlayerGunSelector.Instance.Guns[PlayerGunSelector.Instance.activeGunIndex] == PlayerGunSelector.Instance.Guns[0])
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

            if (PlayerGunSelector.Instance.Guns[PlayerGunSelector.Instance.activeGunIndex] == PlayerGunSelector.Instance.Guns[1])
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
        if (PlayerGunSelector.Instance.Guns[PlayerGunSelector.Instance.activeGunIndex] == PlayerGunSelector.Instance.Guns[1])
        {
            PlayerGunSelector.Instance.SetupNewGun(Gun);
            Destroy(gameObject);
            isImageActivate = false;
            NotificationSystem.Instance.HideGunNotification();
            if (PlayerGunSelector.Instance.Guns.Count >= 2)
            {

                Sprite gunIconTwo = PlayerGunSelector.Instance.Guns[1].GunIcon;
                gunSelector.secondGunIcon.sprite = gunIconTwo;
            }
        }
    }




}
