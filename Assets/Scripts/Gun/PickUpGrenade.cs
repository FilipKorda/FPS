using UnityEngine;

public class PickUpGrenade : MonoBehaviour
{
    [SerializeField] private bool isSmoke;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            
            if (isSmoke)
            {
                GrenadeInventory.Instance.currentSmokeGranatCount++;
            }    
            else
            {
                GrenadeInventory.Instance.currentGranatCount++;
            }
        }

    }
}
