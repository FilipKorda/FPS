using UnityEngine;
using UnityEngine.Localization;

public class OxygenContainer : MonoBehaviour
{
    public LocalizedString localizeStringEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You collet Oxygen Container", 2f);
            MainInventory.Instance.AddOxygenContainer();
            Destroy(gameObject);
        }
    }
}
