using UnityEngine;
using UnityEngine.Localization;

public class FuelCan : MonoBehaviour
{
    public LocalizedString localizeStringEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You collect Fuel Can", 2f);
            MainInventory.Instance.AddFuelCan();
            Destroy(gameObject);
        }
    }
}
