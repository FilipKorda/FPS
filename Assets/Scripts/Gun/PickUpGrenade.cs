using UnityEngine;
using UnityEngine.Localization;

public class PickUpGrenade : MonoBehaviour
{
    [SerializeField] private bool isSmoke;
    [SerializeField] private GrenadeHandler grenadeHandler;
    [SerializeField] private GrenadeDisplayer grenadeDisplayer;

    public LocalizedString localizeStringEventPress;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (isSmoke)
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventPress, $"Pick up 1 {grenadeHandler.smokeGranatPrefab.Name}", 1.0f, grenadeHandler.smokeGranatPrefab.Name);
                Inventory.Instance.AddSmokeGrenade();
            }
            else
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventPress, $"Pick up 1 {grenadeHandler.granatPrefab.Name}", 1.0f, grenadeHandler.smokeGranatPrefab.Name);
                Inventory.Instance.AddGrenade();
            }
            if (grenadeDisplayer != null)
            {
                grenadeDisplayer.UpdateGrenadeCount();
            }
        }

    }
}
