using UnityEngine;
using UnityEngine.Localization;

public class HealthBandage : MonoBehaviour
{
    public LocalizedString localizeStringEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You collect Bandage", 2f);
            MainInventory.Instance.AddHealthBandage();
            Destroy(gameObject);
        }
    }
}
