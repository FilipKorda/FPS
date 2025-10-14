using UnityEngine;
using UnityEngine.Localization;

public class BarrelForTurret : MonoBehaviour
{
    public LocalizedString localizeStringEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You collect Barrel", 2f);
            MainInventory.Instance.AddBarrel();
            Destroy(gameObject);
        }
    }
}
