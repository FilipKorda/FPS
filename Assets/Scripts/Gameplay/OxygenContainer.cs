using UnityEngine;

public class OxygenContainer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            NotificationSystem.Instance.ShowNotification("You collet Oxygen Container", 2f);
            MainInventory.Instance.AddOxygenContainer();
            Destroy(gameObject);
        }
    }
}
