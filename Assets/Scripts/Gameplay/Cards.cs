using UnityEngine;
using UnityEngine.Localization;

public class Cards : MonoBehaviour
{
    public bool isRedCard;
    public bool isGreenCard;
    public bool isBlueCard;

    public LocalizedString localizeStringEventRedCard;
    public LocalizedString localizeStringEventGreenCard;
    public LocalizedString localizeStringEventBlueCard;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isRedCard)
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventRedCard, "You collect Red Card", 2f);
                MainInventory.Instance.AddCard(true, false, false);
                Destroy(gameObject);
            }
            if (isGreenCard)
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventGreenCard, "You collect Green Card", 2f);
                MainInventory.Instance.AddCard(false, true, false);
                Destroy(gameObject);
            }
            if (isBlueCard)
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventBlueCard, "You collect Blue Card", 2f);
                MainInventory.Instance.AddCard(false, false, true);
                Destroy(gameObject);
            }
        }
    }
}
