using UnityEngine;

public class FuelCan : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification("You collect Fuel Can", 2f);
            MainInventory.Instance.AddFuelCan();
            Destroy(gameObject);
        }
    }
}
