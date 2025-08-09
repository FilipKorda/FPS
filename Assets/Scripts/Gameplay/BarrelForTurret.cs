using UnityEngine;

public class BarrelForTurret : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification("You collect Barrel", 2f);
            MainInventory.Instance.AddBarrel();
            Destroy(gameObject);
        }
    }
}
