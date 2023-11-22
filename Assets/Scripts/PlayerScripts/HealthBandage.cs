using UnityEngine;

public class HealthBandage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification("You collect Bandage", 2f);
            MainInventory.Instance.AddHealthBandage();
            Destroy(gameObject);
        }
    }
}
