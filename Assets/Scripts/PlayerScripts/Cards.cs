using UnityEngine;

public class Cards : MonoBehaviour
{
    public bool isRedCard;
    public bool isGreenCard;
    public bool isBlueCard;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isRedCard)
            {
                NotificationSystem.Instance.ShowNotification("You collect Red Card", 2f);
                MainInventory.Instance.AddCard(true,false,false);
                Destroy(gameObject);
            }
            if (isGreenCard)
            {
                NotificationSystem.Instance.ShowNotification("You collect Green Card", 2f);
                MainInventory.Instance.AddCard(false, true, false);
                Destroy(gameObject);
            }
            if (isBlueCard)
            {
                NotificationSystem.Instance.ShowNotification("You collect Blue Card", 2f);
                MainInventory.Instance.AddCard(false, false, true);
                Destroy(gameObject);
            }
        }
    }
}
