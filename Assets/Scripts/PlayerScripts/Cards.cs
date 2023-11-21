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
                MainInventory.Instance.AddCard(true,false,false);
                Destroy(gameObject);
            }
            if (isGreenCard)
            {
                MainInventory.Instance.AddCard(false, true, false);
                Destroy(gameObject);
            }
            if (isBlueCard)
            {
                MainInventory.Instance.AddCard(false, false, true);
                Destroy(gameObject);
            }
        }
    }
}
