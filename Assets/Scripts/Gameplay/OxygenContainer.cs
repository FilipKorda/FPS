using UnityEngine;
using UnityEngine.Localization;

public class OxygenContainer : MonoBehaviour
{
    public LocalizedString localizeStringEvent;
    [SerializeField] private AudioClip pickSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayClip(pickSound, transform.position, 0.01f, true, 1, 500, 1, false, null);
            NotificationSystem.Instance.ShowNotification(localizeStringEvent, "You collet Oxygen Container", 2f);
            MainInventory.Instance.AddOxygenContainer();
            Destroy(gameObject);
        }
    }
}
