using UnityEngine;

public class PickUpGrenade : MonoBehaviour
{
    [SerializeField] private bool isSmoke;
    [SerializeField] private GrenadeHandler grenadeHandler;
    [SerializeField] private GrenadeDisplayer grenadeDisplayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (isSmoke)
            {
                NotificationSystem.Instance.ShowNotification($"Pick up 1 {grenadeHandler.smokeGranatPrefab.Name}", 1.0f);
                Inventory.Instance.AddSmokeGrenade();
            }
            else
            {
                NotificationSystem.Instance.ShowNotification($"Pick up 1 {grenadeHandler.granatPrefab.Name}", 1.0f);
                Inventory.Instance.AddGrenade();
            }
            if (grenadeDisplayer != null)
            {
                grenadeDisplayer.UpdateGrenadeCount();
            }
        }

    }
}
