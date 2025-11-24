using UnityEngine;
using UnityEngine.Localization;

public class FuelCan : MonoBehaviour
{
    public LocalizedString localizeStringEvent;

    [SerializeField] private AudioClip pickSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayClip(pickSound, transform.position, 0.01f, true, 1, 500, 1, false, null);
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You collect Fuel Can", 2f);
            MainInventory.Instance.AddFuelCan();
            Destroy(gameObject);
        }
    }
}
