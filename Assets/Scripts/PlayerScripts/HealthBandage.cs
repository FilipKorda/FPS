using UnityEngine;
using UnityEngine.Localization;

public class HealthBandage : MonoBehaviour
{
    public LocalizedString localizeStringEvent;
    [SerializeField] private AudioClip pickSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayClip(pickSound, transform.position, 0.5f, false, 1, 500, 1, false, null);
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You collect Bandage", 2f);
            MainInventory.Instance.AddHealthBandage();
            Destroy(gameObject);
        }
    }
}
