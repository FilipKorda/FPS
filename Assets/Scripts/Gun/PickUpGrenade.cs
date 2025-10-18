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
                var so = grenadeHandler.smokeGranatPrefab;
                var grenadeName = so.GetLocalizedName();
                NotificationSystem.Instance.ShowNotification(
                    localizeStringEventPress,
                    $"Pick up 1 {grenadeName}",
                    1.0f,
                     grenadeName
                );
                Inventory.Instance.AddSmokeGrenade();
            }
            else
            {
                var so = grenadeHandler.granatPrefab;
                var grenadeName = so.GetLocalizedName();
                NotificationSystem.Instance.ShowNotification(
                    localizeStringEventPress,
                    $"Pick up 1 {grenadeName}",
                    1.0f,
                    grenadeName
                );
                Inventory.Instance.AddGrenade();
            }

            if (grenadeDisplayer != null)
            {
                grenadeDisplayer.UpdateGrenadeCount();
            }
        }
    }
}
